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

Number of testers: 5, the full development team plus an external tester.

Four testers are Computer Science students at Goldsmiths who worked on the project, and an addotional extermal one with no prior knowledge of the game. Each person played independently and gave written feedback on their own. Even though testing was mostly internal, each member worked on a different part of the game so the feedback has covered a big range of things, from art to physics to gameplay and story. The external tester gave the most unbiased responses, with no prior knowledge of the game.

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

Additionally, structured tests were run, covering:
- Full delivery progression from 1 to 8.
- Interaction prompts at pickup and dropoff points.
- Minimap and navigation arrow accuracy
- Food quality penalties
- Environmental collosions
- Dialogue movement restrictions
- Game stability

## 5. Results

Controls were generally picked up without help. Two testers did
not know about the handbrake until they tried it, and one tried 
arrow keys before finding WASD. It was noted that the handbreake 
mechanic is not explained outside of the menu.

The phone dialogue was readable but came up as slightly small on
some screens. Two testers said there was too much dialogue at the
start and they got impatient to drive.

The HUD was considered clear and functional. One tester noted that
the score was not visible, only money. It was noted that food quality 
displaying 100% was confusing when there was no delivery in progress. 
The energy drink mechanic was the most confusing part for testers. 
The timer slowdown wasnot obvious without actively watching the timer, 
and the exactcost of the drink was not clear. 
The buy and skip buttons were flagged as inconsistent because they 
require the mouse while everything else uses the keyboard.

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

All of the following changes were made directly in response to feedback from the playtesting session:
- Score added to the HUD, seen while driving.
- Food quality is now empty when no delivery in progress, removing the confusing 100% display.
- Buy and skip buttons use keyboard keys 1 and 2 instead of requiring the mouse
- Separate popup texts added for tips and quality, to avoid overlap.
- Delivery locations no longer duplicate.
- Minimap camera is zoomed out more.
- Opening dialogue was shortened and updated to explain controls.
- Arrow key movement added alongside WASD.
- AI traffic disabled during dialogue so the player cannot be hit while reading dialogues.
- The game gradually darkens as deliveries progress, making the tone feel darker.
- Food quality penalties extended to all collisions, not just with other cars.
- The car cannot drive through buildings or off the road anymore.
- Ending screen with an ending police arrest scene added for a clear conclusion.
- First delivery made untimed as per the original GDD
- Minimap updated to match the new road layout.
- Fill map completed, with all areas filled in.