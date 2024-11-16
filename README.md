# Fishy

Fishy is a "**work in progress**" vanilla dedicated server for Webfishing written in C#.

> [!CAUTION]
> Fishy is still in early development, so please don’t expect everything to work perfectly just yet!

## Why a dedicated server?
I’ve always loved the concept and style of Webfishing, but there’s one issue that has always bothered me: 
Getting disconnected when the host leaves and never being able to finish the nice chat you have just had with some random person.

**This is where Fishy comes in.**

## About Fishy

Fishy was built entirely from the ground up using a decompiled version of Webfishing, with additional inspiration drawn from [WebFishingCove](https://github.com/DrMeepso/WebFishingCove).

The primary goal of this project is to create a fully functional vanilla dedicated server. 
While mods aren't a primary focus right now, you're welcome to contribute via pull requests if you're interested in exploring that option. 

I’ve made an effort to write clean, maintainable code so that anyone who wants to contribute can easily get involved.

## How to host your own Server
> [!IMPORTANT]
> You will need a second Steam account with the game purchased in order to host your own server.

1. **Install Steam** on your server and download Webfishing.
2. **Extract** the main_zone.tscn from the game ([Example Tool](https://github.com/bruvzg/gdsdecomp))
3. **Download** Fishy from the Releases page
4. **Copy** the main_zone.tscn into the Worlds folder
5. **Modify** the config.toml file to your likings
6. **Start** Fishy

## Similar Projects
- WebfishingCove <https://github.com/DrMeepso/WebFishingCove/>

## TODO

As mentioned earlier, feel free to submit pull requests or open issues if you’d like to contribute! :)

- [x] Complete basic server functionality
- [ ] Improve spawning and events
- [ ] Rework conversion between "GodotPackets" and C#
- [ ] Improve admin system (Reports, Admin-Commands, Banning/Kicking)
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
