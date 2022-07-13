using Microsoft.Bot.Builder;
using Microsoft.Bot.Builder.Dialogs;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace CoffeeBot.Dialogs
{
    public class RootDialog : ComponentDialog
    {
        public RootDialog()
            : base(nameof(RootDialog))
        {

            var steps = new WaterfallStep[]
            {
                PrimeraPregunta,
                Dispatch,
                FinalStepAsync
            };

            AddDialog(new WaterfallDialog(nameof(WaterfallDialog), steps));
            AddDialog(new TextPrompt("PrimeraPregunta"));
            AddDialog(new TestDialog("TestDialog"));
            // The initial child Dialog to run.
            InitialDialogId = nameof(WaterfallDialog);

        }

        private async Task<DialogTurnResult> PrimeraPregunta(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
            return await stepContext.PromptAsync("PrimeraPregunta",
                new PromptOptions
                {
                    Prompt = MessageFactory.Text($"¿Que queres hacer?\n\nEscribe algo...")
                }, cancellationToken);
        }
        private async Task<DialogTurnResult> Dispatch(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
            var text = stepContext.Context.Activity.Text;
            if (text.ToLower() == "test")
                return await stepContext.BeginDialogAsync("TestDialog", cancellationToken: cancellationToken);
            else
                return await stepContext.NextAsync(cancellationToken: cancellationToken);

        }
        private async Task<DialogTurnResult> FinalStepAsync(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
            await stepContext.Context.SendActivityAsync(MessageFactory.Text($"Charla finalizada"));
            return await stepContext.EndDialogAsync(cancellationToken: cancellationToken);
        }
    }
}
