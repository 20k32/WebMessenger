namespace SharpMessenger.Application.UiModels
{
    public static class ButtonDefaults
    {
        public const string ADD_BUTTON_NAME = "Add";
        public const string DELETE_BUTTON_NAME = "Delete";
        public const string ADD_BUTTON_CLASS = "btn btn-success";
        public const string DELETE_BUTTON_CLASS = "btn btn-danger";

        public static Button CreateDeleteButton() => new(DELETE_BUTTON_CLASS, DELETE_BUTTON_NAME);
        public static Button CreateAddButton() => new(ADD_BUTTON_CLASS, ADD_BUTTON_NAME);
    }
}
