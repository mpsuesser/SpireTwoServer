using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ServerSend {
    #region Sender Functions
    private static void SendTCPData(int _toClient, Packet _packet) {
        _packet.WriteLength();
        Server.clients[_toClient].tcp.SendData(_packet);
    }

    private static void SendUDPData(int _toClient, Packet _packet) {
        _packet.WriteLength();
        Server.clients[_toClient].udp.SendData(_packet);
    }

    private static void SendTCPDataToAll(Packet _packet) {
        _packet.WriteLength();
        for (int i = 1; i <= Server.MaxPlayers; i++) {
            Server.clients[i].tcp.SendData(_packet);
        }
    }

    private static void SendTCPDataToAll(int _exceptClient, Packet _packet) {
        _packet.WriteLength();
        for (int i = 1; i < Server.MaxPlayers; i++) {
            if (i != _exceptClient) {
                Server.clients[i].tcp.SendData(_packet);
            }
        }
    }

    private static void SendUDPDataToAll(Packet _packet) {
        _packet.WriteLength();
        for (int i = 1; i <= Server.MaxPlayers; i++) {
            // Debug.Log($"Calling client.udp.SendData to client {i}");
            Server.clients[i].udp.SendData(_packet);
        }
    }

    private static void SendUDPDataToAll(int _exceptClient, Packet _packet) {
        Debug.Log($"Except client specified as: {_exceptClient}");

        _packet.WriteLength();
        for (int i = 1; i < Server.MaxPlayers; i++) {
            if (i != _exceptClient) {
                // Debug.Log($"Sending udp data in except poly to client.udp.SendData {i}");
                Server.clients[i].udp.SendData(_packet);
            }
        }
    }
    #endregion

    public static void WelcomeToServer(int _toClient, bool _slotFree) {
        using (Packet _packet = new Packet((int)ServerPackets.welcomeToServer)) {
            _packet.Write(_toClient);
            _packet.Write(_slotFree);

            Debug.Log($"Sending WelcomeToServer, slotFree: {_slotFree}");
            SendTCPData(_toClient, _packet);
        }
    }

    public static void SelectionAccepted(int _toClient) {
        using (Packet _packet = new Packet((int)ServerPackets.selectionAccepted)) {
            SendTCPData(_toClient, _packet);
        }
    }

    public static void HeroSpawned(int _unitId, int _ownerId, int _heroNum, Vector3 _location, Vector3 _eulerAngles, float _health, float _maxHealth, float _mana, float _maxMana) {
        using (Packet _packet = new Packet((int)ServerPackets.heroSpawned)) {
            _packet.Write(_unitId);
            _packet.Write(_ownerId);
            _packet.Write(_heroNum);
            _packet.Write(_location);
            _packet.Write(_eulerAngles);
            _packet.Write(_health);
            _packet.Write(_maxHealth);
            _packet.Write(_mana);
            _packet.Write(_maxMana);

            SendTCPDataToAll(_packet);
        }
    }

    public static void HeroPositionUpdate(Hero _h) {
        using (Packet _packet = new Packet((int)ServerPackets.heroPositionUpdate)) {
            _packet.Write(_h.ID);
            _packet.Write(_h.transform.position);

            SendUDPDataToAll(_packet);
        }
    }

    public static void HeroStatusUpdate(Hero _h) {
        using (Packet _packet = new Packet((int)ServerPackets.heroStatusUpdate)) {
            _packet.Write(_h.ID);
            _packet.Write(_h.Health);
            _packet.Write(_h.Mana);

            SendTCPDataToAll(_packet);
        }
    }

    public static void EnemySpawnedToAll(Enemy _e) {
        Packet _packet = GetEnemySpawnedPacket(_e);

        SendTCPDataToAll(_packet);
    }

    public static void EnemySpawnedToClient(int _clientId, Enemy _e) {
        Packet _packet = GetEnemySpawnedPacket(_e);

        SendTCPData(_clientId, _packet);
    }

    private static Packet GetEnemySpawnedPacket(Enemy _e) {
        Packet _packet = new Packet((int)ServerPackets.enemySpawned);

        _packet.Write(_e.ID);
        _packet.Write((int)_e.Type);
        _packet.Write((int)_e.State.ID);
        _packet.Write(_e.Health);
        _packet.Write(_e.MaxHealth);
        _packet.Write(_e.gameObject.transform.position);
        _packet.Write(_e.gameObject.transform.rotation);

        return _packet;
    }

    public static void EnemyPositionUpdate(int _enemyId, Vector3 _position, Quaternion _rotation) {
        using (Packet _packet = new Packet((int)ServerPackets.enemyPositionUpdate)) {
            _packet.Write(_enemyId);
            _packet.Write(_position);
            _packet.Write(_rotation);

            SendUDPDataToAll(_packet);
        }
    }

    public static void EnemyStatusUpdate(int _enemyId, float _health) {
        using (Packet _packet = new Packet((int)ServerPackets.enemyStatusUpdate)) {
            _packet.Write(_enemyId);
            _packet.Write(_health);

            SendTCPDataToAll(_packet);
        }
    }

    public static void EnemyKilled(int _enemyId) {
        using (Packet _packet = new Packet((int)ServerPackets.enemyKilled)) {
            _packet.Write(_enemyId);

            SendTCPDataToAll(_packet);
        }
    }

    public static void PlayerDisconnected(int _toClient, int _clientId) {
        using (Packet _packet = new Packet((int)ServerPackets.playerDisconnected)) {
            _packet.Write(_clientId);

            SendTCPData(_toClient, _packet);
        }
    }

    public static void AbilityFired(int _unitId, AbilityID _abilityID, float _cooldown, List<int> _affectedUnitIds = null, List<Vector3> _locationsData = null) {
        using (Packet _packet = new Packet((int)ServerPackets.abilityFired)) {
            _packet.Write(_unitId);
            _packet.Write((int)_abilityID);
            _packet.Write(_cooldown);

            if (_affectedUnitIds != null) {
                _packet.Write(_affectedUnitIds.Count);
                foreach (int id in _affectedUnitIds) {
                    _packet.Write(id);
                }
            } else {
                _packet.Write(0);
            }

            if (_locationsData != null) {
                _packet.Write(_locationsData.Count);
                foreach (Vector3 location in _locationsData) {
                    _packet.Write(location);
                }
            } else {
                _packet.Write(0);
            }

            SendTCPDataToAll(_packet);
        }
    }

    public static void EnemyAbilityFired(int _enemyId, AbilityID _abilityId, List<int> _affectedUnitIds = null, List<Vector3> _locationsData = null) {
        using (Packet _packet = new Packet((int)ServerPackets.enemyAbilityFired)) {
            _packet.Write(_enemyId);
            _packet.Write((int)_abilityId);

            if (_affectedUnitIds != null) {
                _packet.Write(_affectedUnitIds.Count);
                foreach (int id in _affectedUnitIds) {
                    _packet.Write(id);
                }
            } else {
                _packet.Write(0);
            }

            if (_locationsData != null) {
                _packet.Write(_locationsData.Count);
                foreach (Vector3 location in _locationsData) {
                    _packet.Write(location);
                }
            } else {
                _packet.Write(0);
            }

            SendTCPDataToAll(_packet);
        }
    }

    public static void EnemyStateChanged(int _enemyId, EnemyStateID _stateId) {
        using (Packet _packet = new Packet((int)ServerPackets.enemyStateChanged)) {
            _packet.Write(_enemyId);
            _packet.Write((int)_stateId);

            SendTCPDataToAll(_packet);
        }
    }

    public static void SyncDungeonDetailsToOne(int _clientId, DungeonDetails _dd) {
        using (Packet _packet = GetDungeonDetailsPacket(_dd)) {
            SendTCPData(_clientId, _packet);
        }
    }

    public static void SyncDungeonDetailsToAll(DungeonDetails _dd) {
        using (Packet _packet = GetDungeonDetailsPacket(_dd)) {
            SendTCPDataToAll(_packet);
        }
    }

    private static Packet GetDungeonDetailsPacket(DungeonDetails _dd) {
        Packet _packet = new Packet((int)ServerPackets.syncDungeonDetails);

        _packet.Write((int)_dd.ID);
        _packet.Write(GameState.instance.HasStarted);
        _packet.Write(GameState.instance.HasEnded);
        _packet.Write(_dd.TimeElapsed);
        _packet.Write(_dd.EnemiesRequired);
        _packet.Write(_dd.EnemiesKilled);

        _packet.Write(_dd.BossesKilled.Count);
        if (_dd.BossesKilled.Count > 0) {
            foreach (Enemies e in _dd.BossesKilled.Keys) {
                _packet.Write((int)e);
                _packet.Write(_dd.BossesKilled[e]);
            }
        }

        _packet.Write(_dd.MedalTimes.Count);
        if (_dd.MedalTimes.Count > 0) {
            foreach (RunMedal rm in _dd.MedalTimes.Keys) {
                _packet.Write((int)rm);
                _packet.Write(_dd.MedalTimes[rm]);
            }
        }

        return _packet;
    }

    public static void BuffApplied(Buff _buff) {
        using (Packet _packet = new Packet((int)ServerPackets.buffApplied)) {
            _packet.Write(_buff.Attached.ID);
            _packet.Write((int)_buff.ID);
            _packet.Write(_buff.MaxDuration);
            _packet.Write(_buff.DurationRemaining);

            SendTCPDataToAll(_packet);
        }
    }

    public static void BuffPurged(Buff _buff) {
        using (Packet _packet = new Packet((int)ServerPackets.buffPurged)) {
            _packet.Write(_buff.Attached.ID);
            _packet.Write((int)_buff.ID);

            SendTCPDataToAll(_packet);
        }
    }
}
