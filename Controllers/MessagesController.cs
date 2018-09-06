using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using Microsoft.Bot.Connector;

namespace SimpleBot
{
    //teste
    [BotAuthentication]
    public class MessagesController : ApiController
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="activity"></param>
        /// <returns></returns>
        [ResponseType(typeof(void))]
        public virtual async Task<HttpResponseMessage> Post([FromBody] Activity activity)
        {
            if ( activity != null && activity.Type == ActivityTypes.Message)
            {
                await HandleActivityAsync(activity);
            }

            // HTTP 202
            return new HttpResponseMessage(HttpStatusCode.Accepted);
        }

        /// <summary>
        /// Estabelece comunicação entre o usuário e o SimpleBotUser
        /// </summary>
        /// <param name="activity"></param>
        /// <returns></returns>
        async Task HandleActivityAsync(Activity activity)
        {
            string text = activity.Text;
            string userFromId = activity.From.Id;
            string userFromName = activity.From.Name;

            var message = new Message(userFromId, userFromName, text);

            string response = SimpleBotUser.Reply(message);

            await ReplyUserAsync(activity, response);
        }

        /// <summary>
        /// Responde mensagens usando o Bot Framework Connector 
        /// </summary>
        /// <param name="message"></param>
        /// <param name="text"></param>
        /// <returns></returns>
        async Task ReplyUserAsync(Activity message, string text)
        {
            var connector = new ConnectorClient(new Uri(message.ServiceUrl));
            var reply = message.CreateReply(text);

            await connector.Conversations.ReplyToActivityAsync(reply);
        }
    }
}