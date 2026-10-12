namespace Interactive_CarouselView_Gallery.Views
{
    public class FireView : GraphicsView
    {
        private readonly FireDrawable _drawable;

        public FireView()
        {
            _drawable = new FireDrawable();

            Drawable = _drawable;

            // Uuendab animatsiooni umbes 30 korda sekundis.
            Dispatcher.StartTimer(
                TimeSpan.FromMilliseconds(33),
                () =>
                {
                    _drawable.Update();
                    Invalidate();

                    return true; // Animatsioon jätkub.
                });
        }
    }

    public class FireDrawable : IDrawable
    {
        private readonly Random _random = new();
        private readonly List<Particle> _particles = new();

        private float _width;
        private float _height;

        private const int ParticleCount = 65;

        public void Draw(ICanvas canvas, RectF dirtyRect)
        {
            _width = dirtyRect.Width;
            _height = dirtyRect.Height;

            // Loob osakesed ainult esimesel joonistamisel.
            if (_particles.Count == 0 && _width > 0 && _height > 0)
            {
                for (int i = 0; i < ParticleCount; i++)
                {
                    CreateParticle(true);
                }
            }

            // Joonistame sädemed
            foreach (var p in _particles)
            {
                // Hõõguv kuma
                canvas.FillColor = Color.FromRgba(
                    p.R, p.G, p.B, (byte)35);

                canvas.FillEllipse(
                    p.X - p.Size * 2,
                    p.Y - p.Size * 2,
                    p.Size * 4,
                    p.Size * 4);

                // Sädeme heledam keskosa
                canvas.FillColor = Color.FromRgba(
                    p.R, p.G, p.B, (byte)p.Alpha);

                canvas.FillCircle(
                    p.X,
                    p.Y,
                    p.Size);
            }
        }

        public void Update()
        {
            if (_width <= 0 || _height <= 0)
                return;

            foreach (var p in _particles)
            {
                // "Kuum õhk" viib sädemeid üles
                p.Y -= p.Speed;

                // Kerge juhuslik külgsuunaline liikumine.
                p.X += p.Drift;

                // Muudame suunda natuke, et liikumine ei näeks välja sirgjooneline.
                p.Drift += (float)(_random.NextDouble() - 0.5) * 0.08f;
                p.Drift = Math.Clamp(p.Drift, -0.7f, 0.7f);

                // Sädemed hajuvad üles jõudes.
                p.Alpha -= 0.15f;

                // Kadunud säde asendatakse uuega allosas.
                if (p.Y < _height * 0.15f || p.Alpha <= 0)
                {
                    CreateParticle(false, p);
                }
            }
        }

        private void CreateParticle(
            bool randomHeight,
            Particle? existing = null)
        {
            var p = existing ?? new Particle();

            p.X = (float)(_random.NextDouble() * _width);

            p.Y = _height + (float)(_random.NextDouble() * 100);

            p.Size = (float)(1.0 + _random.NextDouble() * 2.5);
            p.Speed = (float)(0.5 + _random.NextDouble() * 1.5);
            p.Drift = (float)(_random.NextDouble() - 0.5) * 0.7f;
            p.Alpha = (float)(140 + _random.NextDouble() * 115);

            // Kuldsed, oranzid ja punased sädemed.
            int color = _random.Next(3);

            if (color == 0)
            {
                p.R = 255;
                p.G = 190;
                p.B = 45;
            }
            else if (color == 1)
            {
                p.R = 255;
                p.G = 90;
                p.B = 20;
            }
            else
            {
                p.R = 220;
                p.G = 30;
                p.B = 25;
            }

            if (existing == null)
            {
                _particles.Add(p);
            }
        }

        private class Particle
        {
            public float X, Y;
            public float Size, Speed, Drift, Alpha;
            public byte R, G, B;
        }
    }
}
