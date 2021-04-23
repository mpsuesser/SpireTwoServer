using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ServerReceive {
    public static void RequestToJoinServer(int _fromClient, Packet _packet) {
        int _clientId = _packet.ReadInt();
        string _username = _packet.ReadString();
        string _version = _packet.ReadString();

        if (_version != Constants.VERSION) {
            Debug.Log($"{_username} tried connecting with out-of-date version {_version}.");
            // ServerSend.VersionOutOfDate(_fromClient);
            return;
        }

        if (!ConfirmClient(_fromClient, _clientId)) {
            return;
        }

        /* if (Server.clients[_fromClient].udpHandshakeRequestReceived) {
            Debug.Log("Scrapping handshake request because already received one from this client.");
        }
        Server.clients[_fromClient].udpHandshakeRequestReceived = true;

        ServerSend.UDPHandshakeReceived(_fromClient, "Here is a UDP message to let you know I've received your UDP message."); */

        Debug.Log("This is where we would return a confirmation that they've joined the lobby.");
    }

    public static void ChooseHero(int _fromClient, Packet _packet) {
        int _clientId = _packet.ReadInt();

        if (!ConfirmClient(_fromClient, _clientId)) {
            return;
        }

        int _heroNum = _packet.ReadInt();

        Debug.Log($"Client has chosen hero #{_heroNum}");

        // Acknowledge the selection has been accepted and that the hero has been spawned.
        ServerSend.SelectionAccepted(_clientId);

        // Spawn hero server-side.
        GameObject g = GameState.instance.SpawnHero(_heroNum, _clientId);
        Hero h = g.GetComponent<Hero>();

        // Send information about our newly spawned hero back to all clients.
        ServerSend.HeroSpawned(h.ID, h.OwnerID, _heroNum, g.transform.position, g.transform.eulerAngles, h.Health, h.MaxHealth, h.Mana, h.MaxMana);

        EventHandler.instance.PlayerJoinedGame(_clientId);
    }

    public static void Movement(int _fromClient, Packet _packet) {
        int _clientId = _packet.ReadInt();
        if (!ConfirmClient(_fromClient, _clientId)) {
            return;
        }

        Movement.Direction _dir = (Movement.Direction)_packet.ReadInt();
        Vector3 _eulerAngles = _packet.ReadVector3();

        EventHandler.instance.UpdateMovement(_clientId, _dir, _eulerAngles);
    }

    public static void JumpInput(int _fromClient, Packet _packet) {
        int _clientId = _packet.ReadInt();
        if (!ConfirmClient(_fromClient, _clientId)) {
            return;
        }

        EventHandler.instance.JumpInput(_clientId);
    }

    public static void AbilityPressed(int _fromClient, Packet _packet) {
        int _clientId = _packet.ReadInt();
        int _ownerId = _packet.ReadInt();
        if (!ConfirmClient(_fromClient, _clientId) || !ConfirmClient(_clientId, _ownerId)) {
            return;
        }

        AbilityID _abilityID = (AbilityID)_packet.ReadInt();

        EventHandler.instance.AbilityPressed(_ownerId, _abilityID);
    }

    public static void DungeonLoaded(int _fromClient, Packet _packet) {
        int _clientId = _packet.ReadInt();
        if (!ConfirmClient(_fromClient, _clientId)) {
            return;
        }

        EventHandler.instance.ClientLoadedDungeon(_clientId);
    }

    #region Helper Functions
    public static bool ConfirmClient(int _fromClient, int _clientId) {
        if (_fromClient != _clientId) {
            Debug.Log($"Player ID {_fromClient} has assumed the wrong client ID: ({_clientId})!");

            return false;
        }

        return true;
    }
    #endregion
}
