using Microsoft.Bot.Builder;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Builder.Dialogs.Choices;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace CoffeeBot.Dialogs
{
    public class TestDialog: ComponentDialog
    {
        public TestDialog(string dialogId)
            : base(dialogId)
        {

            var steps = new WaterfallStep[]
            {
                PrimeraPregunta,
                Dispatch
            };

            AddDialog(new WaterfallDialog(nameof(WaterfallDialog), steps));
            AddDialog(new TextPrompt("PrimeraPregunta"));
            AddDialog(new ChoicePrompt("PreguntaConOpciones"));
            // The initial child Dialog to run.
            InitialDialogId = nameof(WaterfallDialog);

        }

        private async Task<DialogTurnResult> PrimeraPregunta(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
            return await stepContext.PromptAsync("PreguntaConOpciones",
                     new PromptOptions
                     {
                         Prompt = MessageFactory.Text("Que quieres testear:"),
                         Choices = new[] { new Choice("Card"), new Choice("Reply Choice"), new Choice("Atrapar archivo") }
                     }, cancellationToken);
        }

        private async Task<DialogTurnResult> Dispatch(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
            var choice = stepContext.Result as FoundChoice;
            switch (choice.Value)
            {
                case "Card":
                    await stepContext.Context.SendActivityAsync(MessageFactory.Text("enviamos una muestra de card"));
                    break;
                case "Reply Choice":
                    await stepContext.Context.SendActivityAsync(MessageFactory.Text($"Tu texto: {choice.Value}"));
                    break;
                case "Atrapar archivo":
                    await stepContext.Context.SendActivityAsync(MessageFactory.Text($"Aqui te pasamos los datos del archivo que nos pasaste"));
                    break;
                default:
                    break;
            }
            return await stepContext.NextAsync(cancellationToken: cancellationToken);
        }
    }
}
