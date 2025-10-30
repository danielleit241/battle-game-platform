using BattleGame.MessageBus.Events;
using System.Globalization;
using System.Text;

namespace BattleGame.GameService.Search
{
    public class GameEsMapper
    {
        public GameIndexDocument Map(GameCreatedEvent @event)
        {
            return new GameIndexDocument
            {
                Id = @event.GameId,
                Name = @event.GameName,
                Description = @event.GameDes ?? "",
                MaxPlayers = @event.MaxPlayers,
                CreatedAt = @event.CreatedAt,
                UpdatedAt = DateTime.UtcNow,

                Suggest = new SimpleCompletion
                {
                    Input = [NormalizeKeyword(@event.GameName)]
                }
            };
        }

        private string NormalizeKeyword(string s)
        {
            s ??= "";
            var lower = s.Trim().ToLowerInvariant();
            return RemoveDiacritics(lower);
        }

        private string RemoveDiacritics(string text)
        {
            if (string.IsNullOrEmpty(text))
                return text;

            var normalizedString = text.Normalize(NormalizationForm.FormD);
            var sb = new StringBuilder(normalizedString.Length);
            foreach (var c in normalizedString)
            {
                if (CharUnicodeInfo.GetUnicodeCategory(c) != UnicodeCategory.NonSpacingMark)
                {
                    sb.Append(c);
                }
            }

            return sb.ToString().Normalize(NormalizationForm.FormC);
        }
    }
}
