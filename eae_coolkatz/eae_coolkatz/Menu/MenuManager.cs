﻿using eae_coolkatz.Screens;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace eae_coolkatz.Menu
{
    public class MenuManager
    {
        Menu menu;
        bool isTransitioning;

        public MenuManager()
        {
            menu = new Menu();
            menu.OnMenuChange += Menu_OnMenuChange;
        }

        private void Menu_OnMenuChange(object sender, EventArgs e)
        {
            XmlManager<Menu> xmlMenuManager = new XmlManager<Menu>();
            menu.UnloadContent();
            menu = xmlMenuManager.Load(menu.ID);
            menu.LoadContent();
            menu.OnMenuChange += Menu_OnMenuChange;
            menu.Transition(0.0f);

            foreach(MenuItem item in menu.Items)
            {
                item.Image.StoreEffects();
                item.Image.ActivateEffect("FadeEffect");
            }
        }

        void Transition(GameTime gameTime)
        {
            if(isTransitioning)
            {
                int oldMenuCount = menu.Items.Count;
                for(int i = 0; i < oldMenuCount; i++)
                {
                    menu.Items[i].Image.Update(gameTime);
                    float first = menu.Items[0].Image.Alpha;
                    float last = menu.Items[menu.Items.Count - 1].Image.Alpha;
                    if (first == 0.0f && last == 0.0f)
                    {
                        menu.ID = menu.Items[menu.ItemNumber].LinkId;
                    }
                    else if(first == 1.0f && last == 1.0f)
                    {
                        isTransitioning = false;
                        foreach(MenuItem item in menu.Items)
                        {
                             item.Image.RestoreEffects();
                        }
                    }
                        
                }
            }
        }

        public void LoadContent(string menuPath)
        {
            if(menuPath != string.Empty)
            {
                menu.ID = menuPath;
            }
        }

        public void UnloadContent()
        {
            menu.UnloadContent();

        }

        public void Update(GameTime gameTime)
        {
            if(!isTransitioning)
            {
                menu.Update(gameTime);
            }
            if(InputManager.Instance.KeyPressed(Keys.Enter) && !isTransitioning)
            {
                if(menu.Items[menu.ItemNumber].LinkType == "Screen")
                {
                    ScreenManager.Instance.ChangeScreens(menu.Items[menu.ItemNumber].LinkId);
                }
                else if(menu.Items[menu.ItemNumber].LinkType == "Exit")
                {
                    ScreenManager.Instance.Exit = true;
                }
                else
                {
                    isTransitioning = true;
                    menu.Transition(1.0f);
                    foreach(MenuItem item in menu.Items)
                    {
                        item.Image.StoreEffects();
                        item.Image.ActivateEffect("FadeEffect");
                    }
                }
            }
            Transition(gameTime);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            menu.Draw(spriteBatch);
        }
    }
}
