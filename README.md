# AdofaiSRM
AdofaiSRM is a Song Request Manager which allows twitch chat to request songs

## Instalation Instruction:
- Create a folder called `CustomSongs` in the adofai base directory (`~\A Dance of Fire and Ice\CustomSongs`
- Open the `info.json` file located in `~\A Dance of Fire and Ice\Mods\AdofaiSRM\`
- Go to [https://twitchapps.com/tmi/](https://twitchapps.com/tmi/) to get your twitch oauth code
- Paste to oauth into the `TwitchKey` field inside the `info.json` file
- Add your channel name to the `Channels` array

## Usage
#### Streamer
In the `Featured Levels` portal there should be a song called `Song Request Manager`, enter this song and you'll be taken into the request queue

#### Twitch Chat
There are currently 2 command you can use, more are to be added in the future
- `!help`
  - Shows all available commands
  - Example Usage
    - !help
- `!srm <code>`
  - This command adds a song to the queue, you can use both [steam](https://steamcommunity.com/app/977950/workshop/) and [adofai.gg](https://adofai.gg/) codes
  - Example Usage
    - !srm 3704
    - !srm 2804370147
- `!queue <?page> `
  - Show all songs currently in the queue
  - Example Usage
    - !queue
    - !queue 3
