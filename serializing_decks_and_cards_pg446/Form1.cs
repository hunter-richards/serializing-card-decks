﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace serializing_decks_and_cards_pg446
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        Random random = new Random();
        private Deck RandomDeck(int number)
        {
            Deck myDeck = new Deck(new Card[] { });
            for (int i = 0; i < number; i++)
            {
                myDeck.Add(new Card(
                    (Suits)random.Next(4),
                    (Values)random.Next(1, 14)));
            }
            return myDeck;
        }

        private void DealCards(Deck deckToDeal, string title)
        {
            Console.WriteLine(title);
            while (deckToDeal.Count > 0)
            {
                Card nextCard = deckToDeal.Deal(0);
                Console.WriteLine(nextCard.Name);
            }
            Console.WriteLine("-----------------------");
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Deck deckToWrite = RandomDeck(5);
            using (Stream output = File.Create("Deck1.dat"))
            {
                BinaryFormatter bf = new BinaryFormatter();
                bf.Serialize(output, deckToWrite);
            }
            DealCards(deckToWrite, "What I just wrote to the file");
        }

        private void button2_Click(object sender, EventArgs e)
        {
            using (Stream input = File.OpenRead("Deck1.dat"))
            {
                BinaryFormatter bf = new BinaryFormatter();
                Deck deckFromFile = (Deck)bf.Deserialize(input);
                DealCards(deckFromFile, "What I read from the file");
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            using (Stream output = File.Create("Deck2.dat"))
            {
                BinaryFormatter bf = new BinaryFormatter();
                for (int i = 1; i <= 5; i++)
                {
                    Deck deckToWrite = RandomDeck(random.Next(1, 10));
                    bf.Serialize(output, deckToWrite);
                    DealCards(deckToWrite, "Deck #" + i + "written");
                }
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            using (Stream input = File.OpenRead("Deck2.dat"))
            {
                BinaryFormatter bf = new BinaryFormatter();
                for (int i = 1; i <= 5; i++)
                {
                    Deck deckToRead = (Deck)bf.Deserialize(input);
                    DealCards(deckToRead, "Deck #" + i + " read");
                }
            }
        }

        private void pg453_Click(object sender, EventArgs e)
        {
            Card threeClubs = new Card((Suits)1,(Values)3);
            Card sixHearts = new Card((Suits)3, (Values)6);

            using (Stream output1 = File.Create("three-c.dat"))
            {
                BinaryFormatter bf1 = new BinaryFormatter();
                bf1.Serialize(output1, threeClubs);
            }
            using (Stream output2 = File.Create("six-h.dat"))
            {
                BinaryFormatter bf2 = new BinaryFormatter();
                bf2.Serialize(output2, sixHearts);
            }
            
            //loop to compare the files
            byte[] firstFile = File.ReadAllBytes("three-c.dat");
            byte[] secondFile = File.ReadAllBytes("six-h.dat");
            for (int i = 0; i < firstFile.Length; i++)
                if (firstFile[i] != secondFile[i])
                    Console.WriteLine("Byte #{0}: {1} versus {2}",
                        i, firstFile[i], secondFile[i]);

            //manually create a new file that contains the King of Spades
            firstFile[362] = (byte)Suits.Spades;
            firstFile[430] = (byte)Values.King;
            File.Delete("king-s.dat");
            File.WriteAllBytes("king-s.dat", firstFile);
            
            //print out deserialized King of Spades to verify
            using (Stream output3 = File.OpenRead("king-s.dat"))
            {
                BinaryFormatter bf3 = new BinaryFormatter();
                Card alteredCard = (Card)bf3.Deserialize(output3);
                Console.WriteLine("The deserialized new card is the " + alteredCard.Value +
                    " of " + alteredCard.Suit);
            }

            
        }
    }
}
