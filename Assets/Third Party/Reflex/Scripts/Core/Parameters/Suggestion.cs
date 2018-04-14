namespace ReflexCLI.Parameters
{
	public class Suggestion
	{
		public string Value;
		public string Display;

		public Suggestion(string value, string display)
		{
			Value = value;
			Display = display;
		}

		public static implicit operator Suggestion(string value)
		{
			return new Suggestion(value, value);
		}
	}
}