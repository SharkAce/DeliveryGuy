# Delivery Guy #12 - Playtesting Plan

## 1. Objectives

The goal of playtesting was to find issues with controls, pacing
and UI before we submit. We wanted to answer these questions:

1. Could we pick up the controls within the first delivery
   without being told?
2. Is the phone UI dialogue readable and easy to interact with?
3. Does the timer and HUD give enough information while driving?
4. Does the energy drink mechanic in delivery 6 feel clear
   and impactful?
5. Is the overall tone and atmosphere coming through even though
   the game is not fully complete yet?
6. Does the dialogue feel natural and fit the dark humour of the game?
7. Are the organ pun restaurant names noticeable and do they hint
   that something is off?
8. Does the phone UI get in the way of driving?
9. Is the tip and money system easy to understand without explanation?
10. At what point did you first feel something was wrong
    with the deliveries?

## 2. Participants

Number of testers: 4, the full development team.

All testers are Computer Science students at Goldsmiths who worked
on the project. Each person played independently and gave written
feedback on their own. Even though it was internal testing, each
team member worked on a different part of the game so the feedback
covered a good range of things, from art and physics to gameplay
and story.

## 3. Method

Each tester played through the available deliveries on their own,
with no hints or help from anyone else on the team. After playing,
they answered the questions from section 1 in writing via our
group chat. We then looked at the responses together and noted
what kept coming up across multiple testers.

No formal session was set up. Testing happened in the last week
of development when enough of the game was playable to get
useful feedback.

## 4. Test scenarios

No specific tasks were given. Everyone played through the game
from the beginning and got as far as they could.

We paid attention to:

- Whether the controls felt natural from the start
- How we reacted to the phone dialogue and whether we read it
- Whether the HUD gave enough information while driving
- How delivery 6 felt with the energy drink mechanic
- Whether any of the restaurant names stood out

## 5. Results

Controls were generally picked up without help. Two testers did
not know about the handbrake until they tried it, and one tried
arrow keys before finding WASD. The controls worked but supporting
arrow keys as well would make them more accessible.

The phone dialogue was readable but came up as slightly small on
some screens. Two testers said there was too much dialogue at the
start and they got impatient to drive.

The HUD was considered clear and functional. One tester noted that
the score was not visible, only money. The energy drink mechanic
was the most confusing part for testers. The timer slowdown was
not obvious without actively watching the timer, and the exact
cost of the drink was not clear. The buy and skip buttons were
flagged as inconsistent because they require the mouse while
everything else uses the keyboard.

The tone and atmosphere came through for most testers but the
shift to darker content felt sudden. The dialogue was well received
and the boss character was noted as feeling natural and consistent.

The restaurant names were noticed and appreciated but only one
tester connected them to the twist. Most thought they were just
a funny touch, which is okay.

The phone did not obstruct driving. The tip and money system was
partially understood. Testers knew they got money after deliveries
but the connection between driving quality and tip amount was not
clear.

The twist expectation varied. One tester felt something was off after
delivery 4 when Jonnas acted strangely. Another only felt it at
the very end. One tester noted that the game has no ending screen,
which left them confused about whether the game had finished.

Egzon's structured test identified two critical issues. Food quality
does not drop when hitting pedestrians, parked cars, or buildings,
only moving traffic. The car can also drive through buildings and
off the road entirely. Both are collision issues flagged as critical
priority.

Testers also raised the following questions during testing: why do
the first two deliveries use the same location, why does the timer
not reset between deliveries without dialogue, why are some areas
of the map grey, and what happens when time runs out.

## 6. Changes made based on feedback

The following were identified as priorities based on the feedback:
- The buy and skip buttons could use keyboard input to be consistent with the rest of the controls
- An ending screen needs to be added so the game has a clear conclusion
- The minimap zoom needs adjusting so destination markers appear earlier while driving
- The starting location needs protecting from AI traffic so the car cannot be hit during the opening dialogue
- Food quality penalties need to apply to all collision types, not just moving traffic
- Environmental colliders need adding so the car cannot drive through buildings or off the road