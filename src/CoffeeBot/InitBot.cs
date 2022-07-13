// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using Microsoft.Bot.Builder;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Schema;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace CoffeeBot
{
    public class InitBot<T> : ActivityHandler where T : Microsoft.Bot.Builder.Dialogs.Dialog
    {
        protected readonly Microsoft.Bot.Builder.Dialogs.Dialog _dialog;
        private readonly IStatePropertyAccessor<DialogState> _dialogStateProfileAccessor;
        private readonly ConversationState _conversationState;

        public InitBot(T dialog, ConversationState conversationState)
        {
            _dialog = dialog;
            _conversationState = conversationState;
            _dialogStateProfileAccessor = _conversationState.CreateProperty<DialogState>(nameof(DialogState));
        }

        protected override async Task OnMembersAddedAsync(IList<ChannelAccount> membersAdded, ITurnContext<IConversationUpdateActivity> turnContext, CancellationToken cancellationToken)
        {
            foreach (var member in membersAdded)
            {
                if (member.Id != turnContext.Activity.Recipient.Id)
                {
                    await turnContext.SendActivityAsync(MessageFactory.Text($"Hello world!"), cancellationToken);
                }
            }
        }

        //Para atrapar todos los mensjaes que llegan al bot antes de continuar con el dialogo
        protected override async Task OnMessageActivityAsync(ITurnContext<IMessageActivity> turnContext, CancellationToken cancellationToken)
        {
            var mensaje = turnContext.Activity.Text;

            //await turnContext.SendActivityAsync(MessageFactory.Text($"# Hola, este mensaje se envia por defecto. **{mensaje}**"), cancellationToken);
            await _dialog.RunAsync(turnContext, _dialogStateProfileAccessor, cancellationToken);
        }
        
        //Para guarda el estado de la conversacion
        public override async Task OnTurnAsync(ITurnContext turnContext, CancellationToken cancellationToken = default)
        {
            await base.OnTurnAsync(turnContext, cancellationToken);
            await _conversationState.SaveChangesAsync(turnContext, cancellationToken: cancellationToken);
        }

    }
}
