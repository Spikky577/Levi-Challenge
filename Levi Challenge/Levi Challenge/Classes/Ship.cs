﻿using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Levi_Challenge
{
    class Ship
    {
        public string Name;
        public int Health;
        public float Shield;
        public float Speed;
        public int WeaponClass;
        public int Armour;
        public string ShipTexturePath;
        public int Cost;
        public string ShipType;
        public int Level;
        public int Points;
        public string AI;
        public Weapon[] MountPoints;
        public Texture2D ShipTexture;

        private int HardPoints;

        // Player Constructor
        public Ship(string name, int health, float shield, float speed, int weaponclass, int armour, string texture, int cost, string shiptype, int hardpoints)
        {
            Name = name;
            Health = health;
            Shield = shield;
            Speed = speed;
            WeaponClass = weaponclass;
            Armour = armour;
            ShipTexturePath = texture;
            Cost = cost;
            ShipType = shiptype;

            MountPoints = new Weapon[hardpoints];
        }

        // Enemy Constructor
        public Ship(string name, int health, float shield, float speed, int weaponclass, int armour, string texture, int level, int points, string ai, int hardpoints)
        {
            Name = name;
            Health = health;
            Shield = shield;
            Speed = speed;
            WeaponClass = weaponclass;
            Armour = armour;
            ShipTexturePath = texture;
            Level = level;
            Points = points;
            AI = ai;

            MountPoints = new Weapon[hardpoints];
        }

        public void MountWeapon(int mountpoint, Weapon weapon)
        {
            MountPoints[mountpoint] = weapon;
        }

        public void FireWeapon(GameTime gameTime, ProjectileManager projectileManager, Vector2 position)
        {
            for (int mPoint = 0; mPoint < MountPoints.Length; mPoint++)
                MountPoints[mPoint].fire(gameTime, projectileManager, position);
        }
    }
}
