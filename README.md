# SPT Mumble Link

This is a plugin for SPT and FIKA to transmit your character's location to Mumble to enable positional audio.

## How it works

All players must have the plugin installed to transmit their position.

```
Bob, Alice, and Thomas are all in the same raid and alive:
✅<Bob> can hear directions and distance for <Alice> and <Thomas>.
✅<Alice> can hear directions and distance for <Bob> and <Thomas>.
✅<Thomas> can hear directions and distance for <Bob> and <Alice>.

Bob is in the raid, but Alice and Thomas are dead:
❌<Bob> can hear without directions and distance for <Alice> and <Thomas>.
❌<Alice> can hear without directions and distance for <Bob> and <Thomas>.
❌<Thomas> can hear without directions and distance for <Bob> and <Alice>.

Alice has left the raid (extracted), and Bob and Thomas are still alive:
✅<Bob> can hear directions and distance for <Thomas>.
❌<Bob> can hear without directions and distance for <Alice>.
✅<Thomas> can hear directions and distance for <Bob>.
❌<Thomas> can hear without directions and distance for <Alice>.
❌<Alice> can hear without directions and distance for <Bob> and <Thomas>.

Thomas is dead, Bob is in the raid, and Alice has left the raid:
❌<Bob> can hear without directions and distance for <Alice> and <Thomas>.
❌<Alice> can hear without directions and distance for <Bob> and <Thomas>.
❌<Thomas> can hear without directions and distance for <Bob> and <Alice>.

All are out of the raid (either dead or extracted):
❌<Bob>, <Alice>, and <Thomas> can all hear each other without directions and distance.
```

## How to install

1. Make sure that the [Fika Plugin](https://github.com/project-fika/Fika-Plugin) has been installed.
2. Drop the content of the archive file into the root of your SPT folder.

## How to setup Mumble

1. Go to settings

2. Under "Audio Output" and then "Positional Audio" make sure that "Enable" is ticked.
![image](https://github.com/user-attachments/assets/d3480b79-021b-4271-8e13-a2eb2f17ff86)

3. Under "Plugins" and "Options" make sure that "Link to Game and Transmit Position" is ticked.

4. Under "Plugins" and "Plugins" make sure the "Link" entry has been enabled and that "PA" is enabled as well.
![image](https://github.com/user-attachments/assets/0b6b35f7-cf58-41f0-8ab0-29dc2b530d95)

## Recommended Mumble positional settings

* Set the "Minimum Volume" to 50 or 60% to make sure that you can install hear your friends at a distance.
* Set a "Maximum Distance" to around 50m to create a smooth transition for the volume to gradually lower as they move to prevent the volume dropping too fast.
![image](https://github.com/user-attachments/assets/2543ebd4-595e-44ce-b470-67af667bfb70)

