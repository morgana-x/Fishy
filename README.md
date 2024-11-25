# Fishy

Fishy is a "**work in progress**" vanilla dedicated server for Webfishing written in C#.

> [!WARNING]
> Fishy is in Beta—it's largely stable, but some minor issues may occur, and a few features are still in progress.

## Why a dedicated server?
I’ve always loved the concept and style of Webfishing, but there’s one issue that has always bothered me: 
Getting disconnected when the host leaves and never being able to finish the nice chat you have just had with some random person.

**This is where Fishy comes in.**

## About Fishy

Fishy was built entirely from the ground up using a decompiled version of Webfishing, with additional inspiration drawn from [WebFishingCove](https://github.com/DrMeepso/WebFishingCove).

The primary goal of this project is to create a fully functional vanilla dedicated server. 
While mods aren't a primary focus right now, you're welcome to contribute via pull requests if you're interested in exploring that option. 

I’ve made an effort to write clean, maintainable code so that anyone who wants to contribute can easily get involved.

## Features

- Near-complete actor support
- Highly customizable via config.toml
- Player and admin commands
- Extensions support (webserver plugin included)

## How to host your own Server
> [!IMPORTANT]
> You will need a second Steam account with the game purchased in order to host your own server.

1. **Install Steam** on your server and download Webfishing
2. **Extract** the main_zone.tscn from the game ([Example Tool](https://github.com/bruvzg/gdsdecomp))
3. **Download** Fishy from the Releases page
4. **Copy** the main_zone.tscn into the Worlds folder
5. **Modify** the config.toml file to your likings
6. **Start** Fishy

**Using the Webserver plugin?** Visit [http://localhost:8080](http://localhost:8080) and log in with the credentials from config.toml

## How to create your own Extensions

- Reference Fishy.dll
- Create a main plugin class inheriting from FishyExtension
- Override functions to add custom logic
- Compile the plugin into a single .dll (e.g., using Costura.Fody)
- Add the .dll to the Plugins folder

## Similar Projects
- WebfishingCove <https://github.com/DrMeepso/WebFishingCove/>

## TODO

As mentioned earlier, feel free to submit pull requests or open issues if you’d like to contribute! :)

- [x] Complete basic server functionality
- [ ] Release Linux builds
- [x] Improve spawning and events
- [ ] Rework conversion between "GodotPackets" and C#
- [x] Improve admin system (Reports, Admin-Commands, Banning/Kicking)
- [ ] Address bug fixes (and even more bug fixes)

## License
Copyright (C) 2024 ncrypted.dev

This program is free software: you can redistribute it and/or modify
it under the terms of the GNU General Public License as published by
the Free Software Foundation, either version 3 of the License, or
(at your option) any later version.

This program is distributed in the hope that it will be useful,
but WITHOUT ANY WARRANTY; without even the implied warranty of
MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
GNU General Public License for more details.

You should have received a copy of the GNU General Public License
along with this program.  If not, see <https://www.gnu.org/licenses/>.

## Support me and my projects 

### Liberapay
<a href="https://liberapay.com/ncrypted-dev/donate"><img alt="Donate using Liberapay" src="https://liberapay.com/assets/widgets/donate.svg"></a>
