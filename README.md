# TwitchMIDI



https://github.com/user-attachments/assets/0e6a8ab5-3eb4-4e2e-b433-4b27d56b336b



Allow twitch chat to change soundfonts from MIDI playback in realtime.

This Project utilizes [TwitchLIB 3.5.3](https://www.nuget.org/packages/TwitchLib/3.5.3/) and [FluidSynth 2.3.5](https://github.com/FluidSynth/fluidsynth)

You can use any MIDI player as long as it has selectable devices to use FluidSynth.</br>

# ⚠️WARNING! THIS DOES NOT WORK OUT OF THE BOX⚠️
You will need to get the OAuth key to your bot, which you get can from the [Twitch Developer Dashboard](dev.twitch.tv)

Replace the BotName string with your Bot's name and replace the ChannelName string with you're channels name.

Next, you will need to get the ID of the chat reward, which you can find [here](https://www.instafluff.tv/TwitchCustomRewardID/?channel=YOURTWITCHCHANNEL), just make sure in the URL that you replace ?channel=YOURTWITCHCHANNEL with your actual channel name.

Then, replace SOUNDFONT_PATH, FLUIDSYNTH_PATH and DEFAULT_SOUNDFONT with their respective paths.

# TODO
I'll make a setup check and compile a build so that you don't have to run this in VS, when I have the time.
