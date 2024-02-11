namespace Client.Dto
{
	public sealed record OnQuestionAnswerChangeEventData(Guid QuestionId, Guid AnswerId)
	{
	}
}
