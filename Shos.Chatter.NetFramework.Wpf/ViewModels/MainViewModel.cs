using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;

namespace Shos.Chatter.NetFramework.Wpf.ViewModels
{
    using Models;

    public class MainViewModel : BindableBase
    {
        class CommandBase : ICommand
        {
            public event EventHandler CanExecuteChanged;

            protected ChatterModel Model { get; private set; }

            public CommandBase(ChatterModel model) => Model = model;

            public virtual bool CanExecute(object parameter) => true;

            public virtual void Execute(object parameter)
            {}
        }

        class AddUserCommandType : CommandBase
        {
            public AddUserCommandType(ChatterModel model) : base(model)
            {}

            public override void Execute(object parameter)
            {
                var name = (string)parameter;
                if (!string.IsNullOrWhiteSpace(name))
                    Model.Add(new Shos.Chatter.NetFramework.Models.UserBase { Name = name }).Wait();
            }
        }

        class UpdateUserCommandType : CommandBase
        {
            public UpdateUserCommandType(ChatterModel model) : base(model)
            { }

            public override void Execute(object parameter)
            {
                var id = (int)parameter;
                var user = Model.Users.FirstOrDefault(u => u.Id == id);
                if (user != null)
                    Model.Update(user).Wait();
            }
        }

        class DeleteUserCommandType : CommandBase
        {
            public DeleteUserCommandType(ChatterModel model) : base(model)
            {}

            public override void Execute(object parameter)
            {
                var id = (int)parameter;
                var user = Model.Users.FirstOrDefault(u => u.Id == id);
                if (user != null)
                    Model.Delete(user).Wait();
            }
        }

        class AddChatCommandType : CommandBase
        {
            public AddChatCommandType(ChatterModel model) : base(model)
            { }

            public override void Execute(object parameter)
            {
                var content = (string)parameter;
                if (!string.IsNullOrWhiteSpace(content))
                    Model.Add(new Shos.Chatter.NetFramework.Models.ChatBase { Content = content }).Wait();
            }
        }

        class UpdateChatCommandType : CommandBase
        {
            public UpdateChatCommandType(ChatterModel model) : base(model)
            { }

            public override void Execute(object parameter)
            {
                var id = (int)parameter;
                var chat = Model.Chats.FirstOrDefault(c => c.Id == id);
                if (chat != null)
                    Model.Update(chat).Wait();
            }
        }

        class DeleteChatCommandType : CommandBase
        {
            public DeleteChatCommandType(ChatterModel model) : base(model)
            { }

            public override void Execute(object parameter)
            {
                var id = (int)parameter;
                var chat = Model.Chats.FirstOrDefault(c => c.Id == id);
                if (chat != null)
                    Model.Delete(chat).Wait();
            }
        }

        ChatterModel model = new ChatterModel();

        public ICommand AddUserCommand    { get; private set; }
        public ICommand UpdateUserCommand { get; private set; }
        public ICommand DeleteUserCommand { get; private set; }

        public ICommand AddChatCommand { get; private set; }
        public ICommand UpdateChatCommand { get; private set; }
        public ICommand DeleteChatCommand { get; private set; }

        public MainViewModel()
        {
            Model.PropertyChanged += (_, e) => RaisePropertyChanged(e.PropertyName);

            AddUserCommand    = new AddUserCommandType   (Model);
            UpdateUserCommand = new UpdateUserCommandType(Model);
            DeleteUserCommand = new DeleteUserCommandType(Model);

            AddChatCommand    = new AddChatCommandType   (Model);
            UpdateChatCommand = new UpdateChatCommandType(Model);
            DeleteChatCommand = new DeleteChatCommandType(Model);

            Model.Start();
        }

        public int UserId {
            get => Model.UserId;
            set => Model.UserId = value;
        }

        public IEnumerable<Shos.Chatter.NetFramework.Models.UserBase> Users => Model.Users;
        public IEnumerable<Shos.Chatter.NetFramework.Models.ChatBase> Chats => Model.Chats;

        public ChatterModel Model { get => model; set => model = value; }
    }
}
