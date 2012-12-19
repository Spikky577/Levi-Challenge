﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.IO;

namespace Levi_Challenge
{
    class XMLEngine
    {
        public static List<Ship> PlayerShips = new List<Ship>(), EnemyShips = new List<Ship>();
        public static List<Weapon> Weapons = new List<Weapon>();

        private static string ReadElement(XmlReader reader, string name)
        {
            string Value = null;
            while (reader.NodeType != XmlNodeType.EndElement)
            {
                reader.Read();
                if ((reader.NodeType == XmlNodeType.Element) && reader.Name == name)
                {
                    while (reader.NodeType != XmlNodeType.EndElement)
                    {
                        if (reader.NodeType == XmlNodeType.Text)
                        {
                            Value = reader.Value;
                        }
                        reader.Read();
                    }
                }
            }
            if (Value != null)
                reader.Read();
            return Value;
        }

        public static void PhraseShipXML()
        {
            if (Directory.Exists(@"Data\Ships\"))
            {
                string[] filePaths = Directory.GetFiles(@"Data\Ships\", "*.xml");
                for (int i = 0; i < filePaths.Length; i++)
                {
                    LoadShips(filePaths[i]);
                }
            }
        }

        public static void PhraseWeaponXML()
        {
            if (Directory.Exists(@"Data\Weapons\"))
            {
                string[] filePaths = Directory.GetFiles(@"Data\Weapons\", "*.xml");
                for (int i = 0; i < filePaths.Length; i++)
                {
                    LoadWeapons(filePaths[i]);
                }
            }
        }

        private static void LoadWeapons(string fileName)
        {
            // Load the XML Document
            XmlReader reader = XmlReader.Create(fileName);

            bool Correct_File = false;

            string WeaponType = null, WeaponName = null, ProjectileTexture = null, ProjectileType = null;
            int WeaponClass = 0, ProjectileDamage = 0;
            float WeaponRefireRate = 0f, ProjectileSpeed = 0f;


            while (reader.Read())
            {
                if ((reader.NodeType == XmlNodeType.Element) && reader.Name == "Weapon")
                {
                    WeaponType = reader.GetAttribute(0);
                    Correct_File = true;
                }

                if (Correct_File == true)
                {
                    while (reader.NodeType != XmlNodeType.EndElement)
                    {
                        reader.Read();
                        while (reader.NodeType != XmlNodeType.EndElement)
                        {
                            // WeaponStats Stuff
                            if ((reader.NodeType == XmlNodeType.Element) && reader.Name == "WeaponStats")
                            {
                                WeaponName = ReadElement(reader, "Name");

                                WeaponClass = Convert.ToInt32(ReadElement(reader, "Class"));

                                WeaponRefireRate = (float)Convert.ToDouble(ReadElement(reader, "RefireRate"));
                            }

                            // ProjectileStats Stuff
                            if ((reader.NodeType == XmlNodeType.Element) && reader.Name == "ProjectileStats")
                            {
                                ProjectileTexture = ReadElement(reader, "Texture");

                                ProjectileType = ReadElement(reader, "Type");

                                ProjectileDamage = Convert.ToInt32(ReadElement(reader, "Damage"));

                                ProjectileSpeed = (float)Convert.ToDouble(ReadElement(reader, "Speed"));
                            }
                            reader.Read();
                        }
                    }
                    if ((reader.NodeType == XmlNodeType.EndElement) && reader.Name == "ProjectileStats")
                        Weapons.Add(new Weapon(WeaponType, WeaponName, WeaponClass, WeaponRefireRate, @"Projectiles\" + ProjectileTexture, ProjectileType, ProjectileDamage, ProjectileSpeed));
                }
            }
            reader.Close();

            if (Correct_File == false)
            {
                // Error code here
            }
        }

        private static void LoadShips(string fileName)
        {
            // Load the XML Document
            XmlReader reader = XmlReader.Create(fileName);

            string Name = null, Texture = null, ShipType = null, AI = null;
            int Health = 0, Hardpoints = 0, WeaponClass = 0, Armour = 0, Cost = 0, Level = 0, Points = 0;
            float Shield = 0f, Speed = 0f;
            List<Weapon>ShipWeapons = new List<Weapon>();
            bool Correct_File = false, isPlayer = false;

            while (reader.Read())
            {
                if ((reader.NodeType == XmlNodeType.Element) && reader.Name == "Ship")
                {
                    if (reader.GetAttribute(0) == "true")
                        isPlayer = true;
                    Correct_File = true;
                }

                if (Correct_File)
                {
                    while (reader.NodeType != XmlNodeType.EndElement)
                    {
                        reader.Read();
                        while (reader.NodeType != XmlNodeType.EndElement)
                        {
                            // General Stuff
                            Name = ReadElement(reader, "Name");

                            Health = Convert.ToInt32(ReadElement(reader, "Health"));

                            Shield = (float)Convert.ToDouble(ReadElement(reader, "Shield"));

                            Speed = (float)Convert.ToDouble(ReadElement(reader, "Speed"));

                            Hardpoints = Convert.ToInt32(ReadElement(reader, "Hardpoints"));

                            // Weapon loading
                            WeaponClass = Convert.ToInt32(ReadElement(reader, "WeaponClass"));

                            string weaponName = ReadElement(reader, "Weapon1");
                                if (Hardpoints > 0)
                                {
                                foreach (Weapon weapon in Weapons)
                                {
                                    if (weapon.WeaponName == weaponName)
                                        ShipWeapons.Add(weapon);
                                }

                                if (Hardpoints > 1)
                                {
                                    weaponName = ReadElement(reader, "Weapon2");
                                    foreach (Weapon weapon in Weapons)
                                    {
                                        if (weapon.WeaponName == weaponName)
                                            ShipWeapons.Add(weapon);
                                    }
                                    if (Hardpoints > 2)
                                    {
                                        weaponName = ReadElement(reader, "Weapon3");
                                        foreach (Weapon weapon in Weapons)
                                        {
                                            if (weapon.WeaponName == weaponName)
                                                ShipWeapons.Add(weapon);
                                        }
                                    }
                                }
                            }

                            Armour = Convert.ToInt32(ReadElement(reader, "Armour"));

                            Texture = ReadElement(reader, "Texture");

                            // Player Stuff
                            if (isPlayer)
                            {
                                Cost = Convert.ToInt32(ReadElement(reader, "Cost"));

                                ShipType = ReadElement(reader, "Type");
                            }

                            // Enemy Stuff
                            else
                            {
                                Level = Convert.ToInt32(ReadElement(reader, "Level"));

                                Points = Convert.ToInt32(ReadElement(reader, "Points"));

                                AI = ReadElement(reader, "AI");
                            }
                            reader.Read();
                        }
                    }
                    if (isPlayer)
                    {
                        Ship playerShip = new Ship(Name, Health, Shield, Speed, WeaponClass, Armour, @"Ships\" + Texture, Cost, ShipType, Hardpoints);
                        int weaponPoint = 0;
                        foreach (Weapon weapon in Weapons)
                        {
                            playerShip.MountWeapon(weaponPoint, weapon);
                            weaponPoint++;
                        }
                        PlayerShips.Add(playerShip);
                    }
                    else
                    {
                        Ship enemyShip = new Ship(Name, Health, Shield, Speed, WeaponClass, Armour, @"Ships\" + Texture, Level, Points, AI, Hardpoints);
                        int weaponPoint = 0;
                        foreach (Weapon weapon in ShipWeapons)
                        {
                            enemyShip.MountWeapon(weaponPoint, weapon);
                            weaponPoint++;
                        }
                        EnemyShips.Add(enemyShip);
                    }
                }
            }
            reader.Close();

            if (Correct_File == false)
            {
                // Error code here
            }
        }
    }
}
