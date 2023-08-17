using CommunityToolkit.Mvvm.Messaging.Messages;
using Globomantics.Domain;

namespace Globomantics.Windows.Messages;

public class TodoDeleteMessage : ValueChangedMessage<Todo>
{
    public TodoDeleteMessage(Todo t) : base(t)
    {

    }
}
