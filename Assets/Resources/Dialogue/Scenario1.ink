VAR lastRollSuccess = -1

-> START

=== START ===
A nighttime haze hinders your abilities to traverse the woods.
What was already a faint path now grows near impossible to traverse.
And with the stress of exhaustion creeping evermore closer, you begin to fear that with each step you will be wandering lost in these woods all night.
* [Press On _ROLL(DSP,10]
    -> press_on
* [Scout Somewhere to Camp _ROLL(VIG,7)]
    -> find_camp

=== press_on ===
~ temp roll = lastRollSuccess
{ roll == 0:
    Your attempts to press on lead you further away from the path. You are now lost.
    -> END
- else:
    Somehow, you are able to follow the path still, and reach a bridge over a small creek.
    -> END
}

=== find_camp ===
~ temp roll = lastRollSuccess
{ roll == 0:
    The ground is rocky, and there are no places out of the open air.
    The trees themselves appear brittle, and you struggle to find any comfortable place to rest for the night
    -> END
- else:
    Amidst the hostile environment, you do see a small, low brush that you can nest under for the evening
    Surrounded by brush and boulders, you can rest here for the night, concealed from any dangers that could be lurking.
    -> END
}
=== STOP ===
-> END