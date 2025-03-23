##Custom Dialogues
This mod let's you add your own custom dialogues and voiceovers.
Supports all animations and moods as of v1.10.48.
Also supports custom animations, just add them to the files!

###📢 How to edit / add dialogue or voiceovers:

####1️⃣ Locate the Files:

Go to: CustomDialogues/plugin/text

Open the .lps file corresponding to the animation type you want to modify.

*If you don't know the animation type / name / mood, you can find out by enabling debug in the mods settings*

####2️⃣ Edit the Dialogue & Sound:

Example entry:

default:|Type#default:|Name#default:|Mood#Happy:|Dialogue#I'm having so much fun~!:|SoundEffect#test.wav:|

Syntax format:

type:|Type#type:|Name#name:|Mood#mood:|Dialogue#your_text_here:|SoundEffect#your_sound.wav:|

####3️⃣ (Optional) Add Custom Sounds:

Place your new .wav files in: CustomDialogues/plugin/sound

###⛔ How to use Settings ⛔

Interval: How fast the loop goes. (milliseconds)

Speech Delay: How quickly before dialogue can be played again. (milliseconds)

Speech Chance: Chance a dialogue will be triggered. (0%-100%)

Debug: Let's you figure out which animation is currently playing so you can add dialogue or voiceover to it.

Sound: Enables voiceovers.