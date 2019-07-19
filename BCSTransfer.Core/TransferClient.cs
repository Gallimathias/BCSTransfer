using BCSTransfer.Core.KlickTippModel;
using BCSTransfer.Core.Model;
using BCSTransfer.Core.PretixModel;
using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace BCSTransfer.Core
{
    public class TransferClient : ITransferClient
    {
        public Organizer Organizer { get; set; }
        public Event Event { get; set; }
        public int TwitterQuestionId { get; set; }

        private readonly Logger logger;
        private readonly IPretixClient pretixClient;
        private readonly IKlickTippClient klickTippClient;
        private CancellationTokenSource tokenSource;

        public TransferClient(IPretixClient pretixClient, IKlickTippClient klickTippClient)
        {
            logger = LogManager.GetCurrentClassLogger();
            this.pretixClient = pretixClient;
            this.klickTippClient = klickTippClient;
        }

        public Task Start()
        {
            logger.Debug("Start Transfer");
            tokenSource = new CancellationTokenSource();
            var task = new Task(async () => await TransferLoop(tokenSource.Token), TaskCreationOptions.LongRunning);
            task.Start(TaskScheduler.Default);
            return task;
        }

        public void Stop()
        {
            logger.Debug("Stop Transfer");
            tokenSource.Cancel();
            tokenSource.Dispose();
            tokenSource = null;
        }

        private async Task TransferContact(Organizer organizer, Event @event)
        {
            logger.Debug("Start Transfer Contacts");
            var orders = await pretixClient.GetOrders(organizer, @event);
            var positions = orders?.SelectMany(o => o.Positions);

            foreach (var position in positions)
            {
                var contact = new Contact()
                {
                    PretixId = position.Id,
                    PretixPositionId = position.Positionid,
                    PretixOrder = position.Order,
                    TwitterHandel = GetTwitterName(position.Answers.FirstOrDefault(a => a.Question == TwitterQuestionId))
                };

                if (string.IsNullOrWhiteSpace(position.AttendeeEmail))
                    position.AttendeeEmail = orders.FirstOrDefault(o => o.Code == position.Order).Email;

                if (await klickTippClient.TryTagUserByMail(position.AttendeeEmail))
                {
                    contact.TagedByKlickTipp = true;
                    continue;
                }

                if (string.IsNullOrWhiteSpace(position.AttendeeEmail))
                    continue;

                var sub = new Subscriber
                {
                    Email = position.AttendeeEmail,
                    TwitterHandel = contact.TwitterHandel
                };

                string keyFirstName = "first_name";
                string keyLastName = "last_name";

                if (position.AttendeeNameParts.TryGetValue("_scheme", out var schema))
                {
                    var raw = schema.Split('_');
                    keyFirstName = raw[0] + "_name";
                    keyLastName = raw[1] + "_name";
                }


                if (position.AttendeeNameParts.TryGetValue(keyFirstName, out var firstName))
                    sub.FirstName = firstName;

                if (position.AttendeeNameParts.TryGetValue(keyLastName, out var lastName))
                    sub.LastName = lastName;

                try
                {
                    await klickTippClient.CreateSubscriber(sub);
                    contact.TagedByKlickTipp = true;
                }
                catch (Exception ex)
                {
                    logger.Trace(ex.Message);
                    logger.Error("Can not create subscriber in KlickTipp");
                }
            }

            logger.Debug("Stop Transfer Contacts");
        }

        private async Task TransferLoop(CancellationToken token)
        {
            while (!token.IsCancellationRequested)
            {
                try
                {
                    logger.Info("Start Transfer");
                    await TransferContact(Organizer, Event);
                    logger.Info("End Transfer");
                }
                catch (TaskCanceledException ex)
                {
                    logger.Info(ex, "End transfer loop while task canceled");
                }
                catch (Exception ex)
                {
                    logger.Error(ex, "Error on TransferLoop " + ex.Message);
                }

                try
                {
                    await Task.Delay(60000, token);
                }
                catch (TaskCanceledException ex)
                {
                    logger.Info(ex, "End transfer loop while task canceled");
                }
            }
        }

        public string GetTwitterName(Answer answer)
        {
            if (answer == null)
                return "";

            var answerText = answer.AnswerText;

            if (string.IsNullOrWhiteSpace(answerText))
                return "";

            if (answerText.Contains("twitter.com/")) //answer is hole id
            {
                var index = answerText.LastIndexOf('/');
                return answerText.Substring(index + 1);
            }

            if (answerText.StartsWith("@")) //answer is complete twitter handel
                return answerText.Substring(1);

            return answerText; //answer is only account name
        }
    }
}
