using Newtonsoft.Json;

public class Answer
{
    public int? Question { get; set; }

    [JsonProperty(PropertyName = "question_identifier")]
    public string QuestionIdentifier { get; set; }

    [JsonProperty(PropertyName ="answer")]
    public string AnswerText { get; set; }

    [JsonProperty(PropertyName = "option_idenfiters")]
    public object[] OptionIdenfiters { get; set; }
    public object[] Options { get; set; }
}
