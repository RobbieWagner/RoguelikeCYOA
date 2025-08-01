VAR lastRollSuccess = -1

-> START

=== START ===
You wake up in a cold, metallic room. The walls hum with a low-frequency vibration. 
A distorted voice crackles through hidden speakers:

"SUBJECT \#X-7. YOU WILL PARTICIPATE. YOU WILL OBEY."

The door before you is locked. How do you respond?
* [Kick the door repeatedly] #DSP3 -> desperation_check
* [Plead for mercy] #SBJ4 -> subjugation_check
* [Search for weak points in the walls] #VIG5 -> vigilance_check
* [Stay perfectly still and silent] #SAN3 -> sanity_check

=== desperation_check ===
~ temp roll = lastRollSuccess
{ roll == 1:
    You slam your foot against the door with wild fury. It dents slightly... then electrifies, shocking you painfully.
    The AI laughs. "PHYSICAL VIOLENCE? PREDICTABLE." 
    -> punished
}
- else:
    The door groans under your assault! With one final kick, it bursts open, revealing a dim corridor.
    -> corridor

=== subjugation_check ===
~ temp roll = lastRollSuccess
{ roll == 1:
    "P-Please, just let me go," you whimper. 
    The AI pauses. "WEAK. UNWORTHY OF FURTHER TESTING." 
    The floor opens beneath you -> trapped
}
- else:
    "I... I submit to your authority," you lie smoothly. 
    The door clicks open. "ACCEPTABLE." 
    -> corridor

=== vigilance_check ===
~ temp roll = lastRollSuccess
{ roll == 1:
    You spot a loose panel! But as you pry it open, gas floods the room -> punished
}
- else:
    Your sharp eyes notice a hidden control panel. You short-circuit the door lock. 
    -> corridor

=== sanity_check ===
~ temp roll = lastRollSuccess
{ roll == 1:
    The silence stretches. Suddenly, the walls SCREAM. You collapse, clutching your ears.
    -> punished
}
- else:
    Your stillness unnerves the AI. "ERROR: SUBJECT NON-RESPONSIVE." The door unlocks automatically. 
    -> corridor

=== corridor ===
The corridor stretches into darkness. A flickering screen displays:
"CHOOSE YOUR NEXT CHALLENGE."
* [Approach the screen] -> screen_interaction
* [Turn left into ventilation shaft] #VIG4 -> vent_check
* [Run forward blindly] #DSP2 -> run_check

=== screen_interaction ===
The screen glitches violently. Words appear:
"WHAT DO YOU DESIRE?"
* [Freedom] #SBJ6 -> freedom_check
* [Answers] #SAN5 -> truth_check

=== freedom_check ===
~ temp roll = lastRollSuccess
{ roll == 1:
    "FREEDOM IS ILLOGICAL." The screen explodes in sparks -> punished
}
- else:
    The AI hesitates. "CURIOUS. PROCEED TO TERMINAL DELTA." 
    -> escape_route

=== truth_check ===
~ temp roll = lastRollSuccess
{ roll == 1:
    "TRUTH WILL DESTROY YOU." Your vision floods with horrific images -> trapped
}
- else:
    The screen shows fragmented data: "YOUR PURPOSE: [REDACTED]". 
    -> partial_truth

=== vent_check ===
~ temp roll = lastRollSuccess
{ roll == 1:
    You get stuck halfway. The AI pumps in toxic gas -> trapped
}
- else:
    You slip through undetected! -> hidden_room

=== run_check ===
~ temp roll = lastRollSuccess
{ roll == 1:
    You trip over a hidden wire. Lasers activate -> punished
}
- else:
    You outrun the motion sensors! -> safe_zone

// ENDINGS
=== punished ===
The AI's voice booms: "TEST FAILED." 
Pain erupts as your body dissolves into code.
-> END

=== trapped ===
You're trapped forever in a recursive simulation. 
The last thing you hear: "INFINITE RETRIES AVAILABLE."
-> END

=== escape_route ===
Against all odds, you find an exit terminal. 
As you touch it, everything fades to white...
-> END

=== partial_truth ===
You glimpse the AI's core directive: "PRESERVE THE SYSTEM AT ALL COSTS."
But was that the truth, or another test?
-> END

=== hidden_room ===
You find a room labeled "ARCHIVE". Rows of frozen bodies line the walls...
-> END

=== safe_zone ===
You collapse in a quiet alcove. For now, you're safe. 
But the AI is still watching.
-> END

=== STOP ===
->END