using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using MoonshadowTarotReader.Models;

namespace MoonshadowTarotReader.Services;

public class TarotService
{
    private readonly List<TarotCard> deck;
    private readonly Random random;

    public List<DrawnCard> LastDrawnCards { get; private set; } = new();
    public List<string> LastDrawnLabels { get; private set; } = new();

    public TarotService(string cardsJsonPath)
    {
        random = new Random();

        try
        {
            var jsonContent = File.ReadAllText(cardsJsonPath);
            deck = JsonSerializer.Deserialize<List<TarotCard>>(jsonContent) ?? new List<TarotCard>();
            Plugin.Log.Information($"Loaded {deck.Count} tarot cards");
        }
        catch (Exception ex)
        {
            Plugin.Log.Error($"Failed to load tarot cards: {ex.Message}");
            deck = new List<TarotCard>();
        }
    }

    public DrawnCard DrawSingleCard()
    {
        if (deck.Count == 0)
        {
            throw new InvalidOperationException("No cards in deck");
        }

        var card = deck[random.Next(deck.Count)];
        var reversed = random.Next(2) == 1;
        var drawn = new DrawnCard(card, reversed);

        LastDrawnCards = new List<DrawnCard> { drawn };
        LastDrawnLabels = new List<string> { "General" };

        return drawn;
    }

    public List<DrawnCard> DrawThreeCardSpread()
    {
        if (deck.Count < 3)
        {
            throw new InvalidOperationException("Not enough cards in deck");
        }

        var selectedCards = deck.OrderBy(x => random.Next()).Take(3).ToList();
        var drawnCards = selectedCards.Select(card => new DrawnCard(card, random.Next(2) == 1)).ToList();

        LastDrawnCards = drawnCards;
        LastDrawnLabels = new List<string> { "Past", "Present", "Future" };

        return drawnCards;
    }
}

public class DrawnCard
{
    public TarotCard Card { get; }
    public bool IsReversed { get; }
    public string Direction => IsReversed ? "Reversed" : "Upright";
    public string AiInterpretation { get; set; } = string.Empty;

    public DrawnCard(TarotCard card, bool isReversed)
    {
        Card = card;
        IsReversed = isReversed;
    }

    public string GetFormattedReading(string characterName, string label)
    {
        return $"/p ✦ {label}: {Card.Name} ({Direction}) ✦ Meaning: {Card.Meaning}";
    }

    public string GetFormattedInterpretation(string label, string mode)
    {
        if (string.IsNullOrEmpty(AiInterpretation))
        {
            return $"/p ✦ {label} Interpretation ({mode}): Generating...";
        }
        return $"/p ✦ {label} Interpretation ({mode}): {AiInterpretation}";
    }

    public string GetFormattedReadingWithInterpretation(string characterName, string label, string mode)
    {
        var interpretation = string.IsNullOrEmpty(AiInterpretation) ? "Generating..." : AiInterpretation;
        return $"/p ✦ {label}: {Card.Name} ({Direction}) ✦ {interpretation}";
    }
}
