namespace SharpMessenger.Domain.UiModels
{
    public struct Button
    {
        public string ButtonClass { get; set; }
        public string ButtonName { get; set; }

        public Button(string buttonClass, string buttonName) =>
            (ButtonClass, ButtonName) = (buttonClass, buttonName);

        public Button() : this(ButtonDefaults.ADD_BUTTON_CLASS, ButtonDefaults.ADD_BUTTON_NAME)
        { }
    }
}
