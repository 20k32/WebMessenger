﻿using SharpMessenger.Application.Contracts;
using SharpMessenger.Application.UiModels;

namespace SharpMessegner.ChatUserInterface.UIModels
{
    public class SearchedItemModel : ISearchedItem
    {
        public Button Button { get; set; }
        public Data UserData { get; set; }

        public SearchedItemModel(Data data, Button button) =>
            (UserData, Button) = (data, button);

        public SearchedItemModel(Data data) : this(data, 
            new Button(ButtonDefaults.ADD_BUTTON_CLASS, ButtonDefaults.ADD_BUTTON_NAME))
        { }

        public SearchedItemModel() : this(new PlainStringData("default_data"),
            new Button(ButtonDefaults.ADD_BUTTON_CLASS, ButtonDefaults.ADD_BUTTON_NAME))
        { }
    }
}
