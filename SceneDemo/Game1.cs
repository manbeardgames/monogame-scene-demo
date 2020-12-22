/* ----------------------------------------------------------------------------
    This is free and unencumbered software released into the public domain.

    Anyone is free to copy, modify, publish, use, compile, sell, or
    distribute this software, either in source code form or as a compiled
    binary, for any purpose, commercial or non-commercial, and by any
    means.

    In jurisdictions that recognize copyright laws, the author or authors
    of this software dedicate any and all copyright interest in the
    software to the public domain.We make this dedication for the benefit
    of the public at large and to the detriment of our heirs and
    successors. We intend this dedication to be an overt act of
    relinquishment in perpetuity of all present and future rights to this
    software under copyright law.

    THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
    EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF
    MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT.
    IN NO EVENT SHALL THE AUTHORS BE LIABLE FOR ANY CLAIM, DAMAGES OR
    OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
    ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
    OTHER DEALINGS IN THE SOFTWARE.

    For more information, please refer to <https://unlicense.org>
---------------------------------------------------------------------------- */

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;

namespace SceneDemo
{
    public class Game1 : Game
    {
        //  Manages the presentation of graphics
        private GraphicsDeviceManager _graphics;

        //  Used for 2D rendering to the screen.
        private SpriteBatch _spriteBatch;

        //  The current scene that is active.
        private Scene _activeScene;

        //  The next scene to switch to.
        private Scene _nextScene;

        /// <summary>
        ///     Gets the state of keyboard input during the previous frame.
        /// </summary>
        public KeyboardState PrevKeyboardState { get; private set; }

        /// <summary>
        ///     Gets the state of keyboard input during the current frame.
        /// </summary>
        public KeyboardState CurKeyboardState { get; private set; }

        /// <summary>
        ///     Creates a new Game1 instance.
        /// </summary>
        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        /// <summary>
        ///     Initializes the game.
        /// </summary>
        protected override void Initialize()
        {
            base.Initialize();

            //  Load the GreenCircleScene as our first scene.
            ChangeScene(new GreenCircleScene(this));
        }

        /// <summary>
        ///     Loads the content for our game.
        /// </summary>
        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            //  Load the global spritefont.
            Content.Load<SpriteFont>("font");
        }

        /// <summary>
        ///     Updates our game.
        /// </summary>
        /// <param name="gameTime">
        ///     A snapshot of the timing values.
        /// </param>
        protected override void Update(GameTime gameTime)
        {
            PrevKeyboardState = CurKeyboardState;
            CurKeyboardState = Keyboard.GetState();

            //  If there is a next scene waiting to be switched to
            //  transition to that scene.
            if (_nextScene != null)
            {
                TransitionScene();
            }

            //  If there is an active scene, update it.
            if (_activeScene != null)
            {
                _activeScene.Update(gameTime);
            }

            base.Update(gameTime);
        }

        /// <summary>
        ///     Draws our game.
        /// </summary>
        /// <param name="gameTime"></param>
        protected override void Draw(GameTime gameTime)
        {
            //  If there is an active scene, draw it.
            if (_activeScene != null)
            {
                _activeScene.BeforeDraw(_spriteBatch, Color.Black);
                _activeScene.Draw(_spriteBatch);
                _activeScene.AfterDraw(_spriteBatch);
            }

            base.Draw(gameTime);
        }

        /// <summary>
        ///     Sets the next scene to switch to.
        /// </summary>
        /// <param name="next">
        ///     The Scene instance to switch to.
        /// </param>
        public void ChangeScene(Scene next)
        {
            //  Only set the _nextScene value if it is not the
            //  same instance as the _activeScene.
            if (_activeScene != next)
            {
                _nextScene = next;
            }
        }

        /// <summary>
        ///     Handles transitioning gracefully from one scene to
        ///     the next.
        /// </summary>
        private void TransitionScene()
        {
            if (_activeScene != null)
            {
                _activeScene.UnloadContent();
            }

            //  Perform a garbage collection to ensure memory is cleared
            GC.Collect();

            //  Set the active scene.
            _activeScene = _nextScene;

            //  Null the next scene value
            _nextScene = null;

            //  If the active scene isn't null, initialize it.
            //  Remember, the Initialize method also calls the LoadContent method
            if (_activeScene != null)
            {
                _activeScene.Initialize();
            }
        }
    }
}
