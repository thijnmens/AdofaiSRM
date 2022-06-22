# AdofaiSRM
AdofaiSRM is a Song Request Manager which allows twitch chat to request songs

## Instalation Instruction:
- [Download](https://github.com/thijnmens/AdofaiSRM/releases/latest) the latest release
- Install the mod through [Unity Mod Manager](https://www.nexusmods.com/site/mods/21)
- Rightclick `AdofaiSRM` and then hit `Open Folder`
  - ![Step 3](https://i.imgur.com/3gC1kSk.png)
- In the folder you will find a file named `config.Json` (or just `config`). open this in your prefered editor
  - ![Step 4](https://i.imgur.com/ei1ZZp3.png)
- Go to [https://twitchapps.com/tmi/](https://twitchapps.com/tmi/)
  - ![Step 5](https://i.imgur.com/LlT0E4u.png)
- Click the `Connect` button and go through the authentication process
- Copy the code that appeared, it should start with `oauth:`
  - ![Step 7](https://i.imgur.com/tYdVvdD.png)
- Paste the code you just coppied into the config file under `twitchToken`
- Replace `your channel name here` with your twitch channel name
  - ![Step 8 & 9](https://i.imgur.com/4L71E9e.png)
- Save and close the config
- Validate the config by launching `server.exe` (or just `server`)
  - Does the window close? Than there is still something wrong with your config, double check everything or DM me on discord (`ThiJNmEnS#6669`)
  - Does the window stay open? Well done everything is set up and good to go!

## Usage
#### Streamer
In the `Featured Levels` portal there should be a song called `Song Request Manager`, enter this song and you'll be taken into the request queue

#### Twitch Chat
There are currently 2 command you can use, more are to be added in the future
- `!srm <code>`
  - This command adds a song to the queue, you can use both steam and [adofai.gg](https://adofai.gg/) codes
  - Example Usage
    - !srm 3704
    - !srm gg:3699
    - !srm steam:2804370147
- `!ping`
  - A very basic commands that displays the delay between twitch and the SRM bot
