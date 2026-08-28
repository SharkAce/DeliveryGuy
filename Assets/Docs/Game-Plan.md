# Delivery Guy #12 — Gameplay and Dialogues

## Notes

- **Restaurant names** are based on organ names:
  - Kinney's Pizza (kidney)
  - The Heart of Town Diner (heart)
  - Corny's (cornea)
  - Marrow's Bakery (bone marrow)
  - Panc's Diner (pancreas)
  - Spleendid Eats (spleen)
  - Vein Street Grill (vein)
  - Lung Fung (lung)
  - Aorta's Kitchen (aorta)
- **To pick up:** stop at restaurant for 3 seconds.
  - After each pickup, an order label shows promptly on screen:
    - Pickup name
    - Delivery address
    - Delivery name
    - Delivery person name (Delivery Guy #12)
- **To deliver:** stop at address for 3 seconds.
  - Client comes out
  - Dialogue / no dialogue
  - Delivery complete
- **Tips**
  - Each delivery has a specific tip amount programmed.
  - Full delivery quality bar = 100% of the tip.
  - Delivery quality bar fell 50% = 50% of the tip.
  - Delivery quality bar fell to 0 = no tip.

---

## Tutorial

**What happens:**

- Long, empty road, arrow pointing up.
- Drive ~20 seconds to reach the city.
- Once the city is reached, a "CITY REACHED" pop-up.
- Phone pops up.

**Boss dialogue** *(press Enter to display each line)*:

> "Hey there, welcome aboard! It took you a while."
> "I'm busy now with… never mind, I'll keep it quick."
> "You've got your first delivery already. Head to Kinney's Pizza."
> "Once you pick it up, deliver it to Mark. You'll see his address on the right side of your screen."

**What happens:**

- Delivery address appears on the right side of the screen.

**Boss dialogue:**

> "Oh, this… my son hacked into your car screen. Don't worry about it, he's just a kid."
> "..."
> "Oh, and one more thing. Drive slowly. The pizza is… thin. That's how Mark likes it."
> "But not too slowly. He's an impatient old man. He doesn't like his…pizza, ya know, cold."
> "You screw it up, he'll get angry. You're too slow, he'll get angry. Damn grumpy old man."

**What happens:**

- Red timer starts counting up.
- Arrow leading to Kinney's Pizza.
- Order label appears on screen:

```text
PICKUP: Kinney's Pizza
DELIVER TO: Mark
ORDER: Thin Crust Margherita
DRIVER: Delivery Guy #12
```

- On arrival, arrow leading to Mark.
- (No delivery quality tracker yet, to be introduced.)
- On arrival to Mark, dialogue.

**Mark dialogue:**

> "Oh, a new guy! Funny, the last one barely lasted a week."
> "Let's see…"
> "..."
> "Looks good, but be careful on the corners next time."
> "I'm feeling generous today. Let me send you off with a big tip."

**What happens:**

- Money in the top goes up by $XX.
- Pop-up "+$XX".
- Client leaves.
- Phone calls, Boss.

**Boss dialogue:**

> "How'd your first delivery go? Don't answer. I'm busy."
> "Mark told me you did well, but he told me he saw you drift on that damn corner, kid."
> "My son will add a tracker to your screen, so you're more careful next time."
> "Don't worry. He's just a kid."
> "..."
> "Anyway, you've got the next delivery, and I've got to go. Be careful, kid."

**What happens:**

- Phone disconnects.
- Delivery quality tracker appears on screen.

---

## Delivery 2

**What happens:**

- Delivery address pop-up.
- Arrow leading to The Heart of Town Diner.
- On arrival, order label:

```text
PICKUP: The Heart of Town Diner
DELIVER TO: N/A
ORDER: Classic Chicken Burger
DRIVER: Delivery Guy #12
```

- Arrow leading to delivery address.
- On arrival, no-name client.

**No-name client:**

> "Good. Bye."

**What happens:**

- Money in the top goes up by $XX.
- Pop-up "+$XX".

---

## Delivery 3

**What happens:**

- Delivery address pop-up.
- Arrow leading to Corny's.
- Order label appears on screen:

```text
PICKUP: Corny's
DELIVER TO: N/A
ORDER: Sweetcorn Chowder
DRIVER: Delivery Guy #12
```

- Quick delivery, no client.
- Money in the top goes up by $XX.
- Pop-up "+$XX".

---

## Delivery 4

**What happens:**

- Phone rings, Boss dialogue.

**Boss dialogue:**

> "Next one's from Lung Fung. They sell decent soup, you know."
> "My wife doesn't let me eat there, she says the waitress' got an eye on me."
> "Anyway, Guy, be careful with the next one. It's…ehm…the soup can spill easily."
> "Deliver it to Jonnas. Skinny, tall guy. You'll know once you see him."

**What happens:**

- Delivery pick-up.
- Order label appears on screen:

```text
PICKUP: Lung Fung
DELIVER TO: Jonnas
ORDER: Chicken Noodle Soup (No Bones)
DRIVER: Delivery Guy #12
```

- Arrival to drop-off.
- Jonnas — extremely tall and thin character.
- Looks normal, but his body shape is slenderman-like.

**Jonnas dialogue:**

> "Just–just leave it here. Actually…wait. Come closer. Quick. Come."
> "Did anyone follow you here?"
> "Never…mind. Gimme the mea–soup. The soup. Forget what I said. Here."

**What happens:**

- Money in the top goes up by $XX.
- Pop-up "+$XX".

---

## Delivery 5

**What happens:**

- Delivery address pop-up.
- Arrow leading to Panc's Diner.
- Order label appears on screen:

```text
PICKUP: Panc's Diner
DELIVER TO: N/A
ORDER: Pancake Stack
DRIVER: Delivery Guy #12
```

- Quick delivery, no dialogue.
- Money in the top goes up by $XX.
- Pop-up "+$XX".

---

## Delivery 6

**What happens:**

- Phone rings, Boss dialogue.

**Boss dialogue:**

> "Next one's urgent, kid. Real urgent."
> "Before you head out… buy an energy drink. Trust me, you'll need it."
> "You've gotta use your own money for it, but if you're too slow, you di–you're fired."
> "I know you're raising money for the surgery, but the money will come."
> "..."
> "And the drink isn't even THAT expensive."

**What happens:**

- Phone screen shows two options:
  - "Buy Energy Drink" *(cost: 51% of current player's money)*
  - "Skip"
- If player selects "Skip", phone rings again immediately.

**Boss dialogue** *(if "Skip")*:

> "...Kid, buy the damn drink. I mean it."
> "This isn't a suggestion. Just…trust me on this one."

**What happens:**

- Phone screen shows one option:
  - "Buy Energy Drink" *(cost: 51% of current player's money)*
- Money drops by $XX.
- Pop-up "-$XX".
- Delivery arrow leads to Spleendid Eats.
- Order label appears on screen:

```text
PICKUP: Spleendid Eats
DELIVER TO: N/A
ORDER: Chilled Order – Keep Sealed
DRIVER: Delivery Guy #12
```

- Upon pick-up, arrow leads to delivery address, timer starts:
  - This time, counting down, not up.
  - Timer starts at 00:10.000 (10 seconds).
  - Flashing red.
  - Timer reaches 5 seconds.
  - Energy drink activates.
  - Timer slows down dramatically (1 second on timer = 20 seconds in real life).

**Client dialogue** *(no name given)*:

> "Finally. Thought you weren't coming."
> "Just–give it to me. Quick. In and out…Please."
> "...Thank you. Now, go. And…forget you were here."

**What happens:**

- Delivery complete.
- Money goes up by $XX.
- Pop-up "+$XX".

---

## Delivery 7

**What happens:**

- Delivery address pop-up.
- Arrow leading to Vein Street Grill.
- Order label appears on screen:

```text
PICKUP: Vein Street Grill
DELIVER TO: N/A
ORDER: Grilled Chicken Wrap
DRIVER: Delivery Guy #12
```

- Quick delivery, no dialogue.
- Money in the top goes up by $XX.
- Pop-up "+$XX".

---

## Delivery 8

**What happens:**

- Phone rings, Boss calling.

**Boss dialogue:**

> "One more. Aorta's Kitchen. Last one. I promise."
> "..."
> "...Actually – kid, listen–"

**What happens:**

- Call cuts off mid-sentence.
- Stressful atmosphere.
- Arrow leading to Aorta's Kitchen.
- Order pickup as normal — order label reads simply: **"HEART"**.
- As player drives toward the address, sirens are heard, and getting louder.
- Player loses control, car stops, police cars come from behind and surround the player.

**Officer dialogue:**

> "Turn off the engine. Hands where I can see them."
> "...Open the container."

**What happens:**

- Screen briefly shows the delivery contents — some kind of meat, medical packaging, label with a name.
- Cut to black.

**Text on screen:**

> "You were never delivering food."
> "Neither were the eleven drivers before you."

**What happens:**

- 11 simple grave stones / ash vases are shown on the screen.
  - Each has writing:
    - Delivery Guy #XX
    - Rest In Peace
- Gun shot.
- 12th grave appears, bigger than the others, "Delivery Guy #12".
- Result screen:
  - Total money made
  - Total time taken
  - Replay option
    - Next playthrough will have a new randomised driver look, same job, same ending.