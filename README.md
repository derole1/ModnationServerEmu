# ModnationServerEmu
A server emu for ModNation Racers


BombServer - Emulates the BombProof matchmaking server that is used by the game, this may work with LBPK without modifcation (though untested)

TODO:
- ~~Connects to directory service~~
- ~~Get a list of services to connect to~~
- ~~Connect to all the initially required services successfully~~
- ~~Request a game list from gamebrowser~~
- Attempt to join a game given by gamebrowser
- Can match up with other players in modspot
- Online races can be hosted
- Other services are implemented. For example, stats, playgroup etc

ModnationServer - Emulates the core HTTP server that stores creations and user stats

TODO:
- ~~Initial connection, get policy~~
- ~~Successful login~~
- Fetch announcements etc
- Fetch user creations
- Implement creation upload, download and searching
- Time attack ghosts, leaderboards
- Implement other features, messaging etc

Emulation of the BombProof system is slow because of lack of documentation and packet logs (They're all SSL encrypted). All work so far on the matchmaking server has been done via reverse engineering the game executable (And as such progress on this will be SLOW)
