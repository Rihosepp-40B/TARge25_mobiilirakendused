using Microsoft.Maui.Controls.Shapes;
using Microsoft.Maui.Layouts;

namespace Example_app;


public partial class TreePage : ContentPage
{
	BoxView treeTrunk, grass, sky, nightSky, nightGrass;
    Ellipse branchLight1, branchDark1, branchLight2, branchDark2, branchLight3, branchDark3, sun, moon;
    
    // Oksa varjud ööks
    Ellipse branchNightL1, branchNightD1, branchNightL2, branchNightD2, branchNightL3, branchNightD3;
    AbsoluteLayout al, treeLayout;
	Picker pickAction;
	Button startAction;
	Label statusLabel, stepLabel;
	private Slider slideOpacity;
	private Stepper stepSpeed;
	private DatePicker datePicker;
	private TimePicker timePicker;
	VerticalStackLayout controllers;
	HorizontalStackLayout actionControl, stepperControl;

    Random random = new Random();
    bool actionRunning = false;

    Color summerLight = Colors.ForestGreen;
    Color summerDark = Colors.DarkGreen;

    Color fallLight = Colors.OrangeRed;
    Color fallDark = Colors.LightGoldenrodYellow;

    Color winterLight = Colors.LightGray;
    Color winterDark = Colors.DarkGray;

    Color springLight = Colors.SpringGreen;
    Color springDark = Colors.PaleGreen;

    Color lightBranchColor = Colors.ForestGreen;
    Color darkBranchColor = Colors.DarkGreen;

    public TreePage()
	{
        skyGrassBuild();
                
        // Päikse andmed funktsioonis
        sunBuild();

        // Kuu andmed funktsioonis
        moonBuild();

        // Juhtpaneeli eraldi funktsioon
        controllersBuild();

        // treeLayout objekti loomine ja sellele väärtuse andmine "konteiner"
        treeLayout = new AbsoluteLayout();
        treeBuild();

        //Kuvatavasse layouti objektide lisamine
        al = new AbsoluteLayout
		{
			Children =
			{
				sky,
                nightSky,
                sun,
                moon,
				grass,
                nightGrass,
				treeLayout,
                controllers,
			}
		};
        Content = al;

        Loaded += TreePage_Loaded;
        
    }

    private void TreePage_Loaded(object sender, EventArgs e)
    {
        // funktsioon et APP käivitamisel õiged alguse parameetrid laeks

        seasionStarter();

        // Arvestame kohe ka hetke kellaajaga
        double sekundid = timePicker.Time.Value.TotalSeconds;

        moveSun(sekundid);
        nightOpacity(sekundid);
    }

    private void controllersBuild()
    {
        List<string> actionMenu = new List<string>
        {
            "Kasva",
            "Õitse",
            "Värise",
            "Langeta",
        };

        pickAction = new Picker
        {
            Title = "Vali tegevus",
            TitleColor = Colors.Red,
            ItemsSource = actionMenu,
            TextColor = Colors.Yellow,
            FontSize = 26,
        };

        startAction = new Button
        {
            Text = "Käivita tegevus",
            TextColor = Colors.Black,
            FontSize = 26,
        };
        startAction.Clicked += startAction_Clicked;

        statusLabel = new Label
        {
            Text = "",
            TextColor = Colors.Red,
            FontSize = 20,
        };

        actionControl = new HorizontalStackLayout
        {
            Children =
            {
                pickAction,
                startAction,
            },
        };

        slideOpacity = new Slider
        {
            Maximum = 1.0,
            Minimum = 0,
            Value = 1.0,
            MinimumTrackColor = Colors.Blue,
            MaximumTrackColor = Colors.Black,
            ThumbColor = Colors.Red,
        };

        slideOpacity.ValueChanged += (sender, e) =>
        {
            branchLight1.Opacity = e.NewValue;
            branchDark1.Opacity = e.NewValue;
            branchLight2.Opacity = e.NewValue;
            branchDark2.Opacity = e.NewValue;
            branchLight3.Opacity = e.NewValue;
            branchDark3.Opacity = e.NewValue;
        };

        stepSpeed = new Stepper
        {
            Maximum = 3,
            Minimum = 0.5,
            Increment = 0.5,
            Value = 1.0,
        };

        stepLabel = new Label
        {
            Text = $"{stepSpeed.Value.ToString()}X",
            TextColor = Colors.Red,
            FontSize = 20,
        };

        stepSpeed.ValueChanged += (sender, e) =>
        {
            stepLabel.Text = $"{e.NewValue.ToString()}X";
        };

        stepperControl = new HorizontalStackLayout
        {
            Children =
            {
                stepSpeed,
                stepLabel
            }
        };

        datePicker = new DatePicker
        {
            Date = DateTime.Now,
            Format = "D",
            TextColor = Colors.Red,
            FontSize= 20
        };

        datePicker.DateSelected += (sender, e) =>
        {
            seasonChanges();
        };

        timePicker = new TimePicker
        {
            Time = DateTime.Now.TimeOfDay,
            Format = "HH:mm",
            TextColor = Colors.Red,
            FontSize = 20
        };

        timePicker.PropertyChanged += (sender, e) =>
        {
            double sekundid = timePicker.Time.Value.TotalSeconds;
            moveSun(sekundid);
            nightOpacity(sekundid);
        };

        controllers = new VerticalStackLayout
        {
            Children =
            {
                actionControl,
                statusLabel,
                slideOpacity,
                stepperControl,
                new BoxView
                {
                    HeightRequest = 500,
                    Color = Colors.Transparent,
                    BackgroundColor = Colors.Transparent
                },
                timePicker,
                datePicker
            }
        };
    }

    private void skyGrassBuild()
    {
        sky = new BoxView
        {
            Color = Colors.DeepSkyBlue
        };

        // Taeva paigutus
        AbsoluteLayout.SetLayoutBounds
            (
                sky,
                new Rect(0, 0, 1, 1)
            );

        AbsoluteLayout.SetLayoutFlags
            (
                sky,
                AbsoluteLayoutFlags.All
           );

        nightSky = new BoxView
        {
            Background = new LinearGradientBrush(
            new GradientStopCollection
                {
                    new GradientStop(Colors.Black, 0.0f),
                    new GradientStop(Colors.Black, 0.4f),
                    new GradientStop(Colors.MidnightBlue, 0.6f),
                    new GradientStop(Colors.Transparent, 1.0f)
                },
                new Point(0, 0),
                new Point(0, 1)
            )
        };

        // Taeva paigutus
        AbsoluteLayout.SetLayoutBounds
            (
                nightSky,
                new Rect(0, 0, 1, 1)
            );

        AbsoluteLayout.SetLayoutFlags
            (
                nightSky,
                AbsoluteLayoutFlags.All
           );

        nightSky.Opacity = 0;


        grass = new BoxView
        {
            Color = Colors.LawnGreen
        };

        // Muru paigutus
        AbsoluteLayout.SetLayoutBounds
            (
                grass,
                new Rect(0, 1, 1, 0.40)
            );

        AbsoluteLayout.SetLayoutFlags
            (
                grass,
                AbsoluteLayoutFlags.All
           );

        nightGrass = new BoxView
        {
            Color = Colors.Black,
            BackgroundColor = Colors.Transparent

        };

        // Muru paigutus
        AbsoluteLayout.SetLayoutBounds
            (
                nightGrass,
                new Rect(0, 1, 1, 0.40)
            );

        AbsoluteLayout.SetLayoutFlags
            (
                nightGrass,
                AbsoluteLayoutFlags.All
           );

        nightGrass.Opacity = 0;
    }

	private void treeBuild()
	{        
        treeTrunk = new BoxView
        {
            Color = Color.FromRgb(43, 22, 8)
        };

        branchLight1 = new Ellipse
        {
            Fill = new SolidColorBrush(lightBranchColor),
        };

        branchDark1 = new Ellipse
        {
            Fill = new SolidColorBrush(darkBranchColor),
        };

        branchLight2 = new Ellipse
        {
            Fill = new SolidColorBrush(lightBranchColor),
        };

        branchDark2 = new Ellipse
        {
            Fill = new SolidColorBrush(darkBranchColor),
        };

        branchLight3 = new Ellipse
        {
            Fill = new SolidColorBrush(lightBranchColor),
        };

        branchDark3 = new Ellipse
        {
            Fill = new SolidColorBrush(darkBranchColor),
        };

        branchNightL1 = new Ellipse
        {
            Fill = new SolidColorBrush(Colors.Black),
            BackgroundColor = Colors.Transparent
        };

        branchNightL2 = new Ellipse
        {
            Fill = new SolidColorBrush(Colors.Black),
            BackgroundColor = Colors.Transparent
        };

        branchNightL3 = new Ellipse
        {
            Fill = new SolidColorBrush(Colors.Black),
            BackgroundColor = Colors.Transparent
        };

        branchNightD1 = new Ellipse
        {
            Fill = new SolidColorBrush(Colors.Black),
            BackgroundColor = Colors.Transparent
        };

        branchNightD2 = new Ellipse
        {
            Fill = new SolidColorBrush(Colors.Black),
            BackgroundColor = Colors.Transparent
        };

        branchNightD3 = new Ellipse
        {
            Fill = new SolidColorBrush(Colors.Black),
            BackgroundColor = Colors.Transparent
        };
        
        treeLayout.Children.Clear();

        // treeLayout parameetrid rotatsioon ja ankru punkti asukoht
        treeLayout.Rotation = 0;
        treeLayout.Scale = 1.0;
        treeLayout.AnchorX = 0.5;
        treeLayout.AnchorY = 1;

        treeLayout.Opacity = 1;

        branchLight1.Opacity = slideOpacity.Value;
        branchDark1.Opacity = slideOpacity.Value;

        branchLight2.Opacity = slideOpacity.Value;
        branchDark2.Opacity = slideOpacity.Value;

        branchLight3.Opacity = slideOpacity.Value;
        branchDark3.Opacity = slideOpacity.Value;


        branchNightL1.Opacity = slideOpacity.Value;
        branchNightD1.Opacity = slideOpacity.Value;

        branchNightL2.Opacity = slideOpacity.Value;
        branchNightD2.Opacity = slideOpacity.Value;

        branchNightL3.Opacity = slideOpacity.Value;
        branchNightD3.Opacity = slideOpacity.Value;

        // Kogu puu konteineri asukoht ja mõõtmed
        AbsoluteLayout.SetLayoutBounds(
            treeLayout,
            new Rect(0.5, 0.65, 300, 500)
        );

        AbsoluteLayout.SetLayoutFlags(
            treeLayout,
            AbsoluteLayoutFlags.PositionProportional
        );

        // Puu objektide lisamine treeLayouti
        treeLayout.Children.Add(treeTrunk);

        treeLayout.Children.Add(branchLight1);
        treeLayout.Children.Add(branchNightL1);

        treeLayout.Children.Add(branchDark1);
        treeLayout.Children.Add(branchNightD1);

        treeLayout.Children.Add(branchLight2);
        treeLayout.Children.Add(branchNightL2);

        treeLayout.Children.Add(branchDark2);
        treeLayout.Children.Add(branchNightD2);

        treeLayout.Children.Add(branchLight3);
        treeLayout.Children.Add(branchNightL3);

        treeLayout.Children.Add(branchDark3);       
        treeLayout.Children.Add(branchNightD3);


        // Tüve asukoht konteineris
        AbsoluteLayout.SetLayoutBounds
            (
                treeTrunk,
                new Rect(0.5, 1, 40, 270)
            );

        AbsoluteLayout.SetLayoutFlags
            (
                treeTrunk,
                AbsoluteLayoutFlags.PositionProportional
            );

        //okste loetelu
        List<View> branches = new List<View>
        {
            branchLight1,
            branchDark1,
            branchLight2,
            branchDark2,
            branchLight3,
            branchDark3,
        };

        List<View> branchesNight = new List<View>
        {
            branchNightL1,
            branchNightD1,
            branchNightL2,
            branchNightD2,
            branchNightL3,
            branchNightD3,
        };

        // Okste paigutus konteineris, iga kord oksad erinevas kohas nö "erinev puu"
        for (int i = 0; i < branches.Count; i++)
        {
            double xKoht = random.Next(2, 8) / 10.0;
            double yKoht = random.Next(4, 8) / 10.0;

            AbsoluteLayout.SetLayoutBounds(
                branches[i],
                new Rect(xKoht, yKoht, 150, 150)
            );

            AbsoluteLayout.SetLayoutFlags(
                branches[i],
                AbsoluteLayoutFlags.PositionProportional
            );

            AbsoluteLayout.SetLayoutBounds(
                branchesNight[i],
                new Rect(xKoht, yKoht, 150, 150)
            );

            AbsoluteLayout.SetLayoutFlags(
                branchesNight[i],
                AbsoluteLayoutFlags.PositionProportional
            );
            branchesNight[i].Opacity = 0;
        }
    }

	private void startAction_Clicked(object sender, EventArgs e)
	{
        if (actionRunning)
        {
            return;
        }

        if (pickAction.SelectedIndex == -1)
		{
			return;
		}
	
		string valitudTegevus = pickAction.SelectedItem.ToString();

		if (valitudTegevus == "Kasva")
		{
            statusLabel.Text = "Puu kasvab!";
            growTree();
		}
		else if (valitudTegevus == "Langeta")
		{
            statusLabel.Text = "Puu langeb!";
            cutTree();
		}
		else if (valitudTegevus == "Värise")
		{
            statusLabel.Text = "Puu väriseb!";
            shakeTree();
		}
		else if (valitudTegevus == "Õitse")
		{
            statusLabel.Text = "Puu õitseb";
            bloomTree();
		}
    }
        
	private async void growTree()
    // Kasvamine toimub 0.25 punkti ja ei ole nupu klõpsimise piirangut (actionRunning)
    {
        uint scaleSpeed = (uint)(3000 / stepSpeed.Value);

        await treeLayout.ScaleTo(treeLayout.Scale + 0.25, scaleSpeed);
        statusLabel.Text = "";
    }
        
    private async void cutTree()
    // Langetamine toimub juhuvalikul vasakule või paremale
    {

        int month = datePicker.Date.Value.Month;
        int hour = timePicker.Time.Value.Hours;

        if ( !(month > 11 || month < 3) || (hour > 16 || hour < 8))
        {
            statusLabel.Text = "Pimedas ja suvel puid ei langetata!!";
            return;
        }
        

        actionRunning = true;

        // 0 vasakule, 1 paremale
        int direction = random.Next(0, 2);

        uint rotateSpeed = (uint)(1000 / stepSpeed.Value);
        uint scaleSpeed = (uint)(2000 / stepSpeed.Value);

        double angle;

        if (direction == 0)
        {
            angle = -90;
        }
        else
        {
            angle = 90;
        }

        // puu kubub
        await treeLayout.RotateTo(angle, rotateSpeed);

        // puu hajub
        await treeLayout.FadeTo(0, rotateSpeed);

        // Uus puu
        treeBuild();

        treeLayout.Scale = 0;


        // Kasvab uus puu
        statusLabel.Text = "Kasvab uus!";

        await treeLayout.ScaleTo(0.5, scaleSpeed);

        statusLabel.Text = "";

        actionRunning = false;
    }

    private void sunBuild()
    {
        sun = new Ellipse
        {
            Fill = new SolidColorBrush(Colors.Yellow)
        };

        // Päikese paigutus
        AbsoluteLayout.SetLayoutBounds
            (
                sun,
                new Rect(0.5, 0.2, 150, 150)
            );

        AbsoluteLayout.SetLayoutFlags
            (
                sun,
                AbsoluteLayoutFlags.PositionProportional
           );

        //if (sun.Rotation > sunNewRotation)
        //{
        //    sunNewRotation = 360 - sunNewRotation;
        //}
        sun.AnchorX = 0.5;
        sun.AnchorY = 2.5;
    }

    private void moveSun(double sekundid)
    {
        uint rotateSpeed = (uint)(1000 / stepSpeed.Value);
        
        //sekundid - 43200, muudab kella 12 0 kraadiks
        double sunNewRotation = (sekundid - 43200) / 240;

        sun.RotateTo(sunNewRotation, rotateSpeed);

    }

    private void seasionStarter()
    {
        int month = datePicker.Date.Value.Month;

        if (month >= 6 && month <= 8)
        {
            sun.TranslationY = 0;
            sun.Fill = Color.FromRgb(255, 220, 0);
            nightSky.TranslationY = -150;
            sky.Color = Colors.DeepSkyBlue;

            grass.Color = Colors.LawnGreen;

            lightBranchColor = summerLight;
            darkBranchColor = summerDark;
        }
        else if (month >= 9 && month <= 11)
        {
            sun.TranslationY = 75;
            sun.Fill = Color.FromRgb(237, 175, 79);
            nightSky.TranslationY = -75;
            sky.Color = Colors.LightSkyBlue;

            grass.Color = Colors.DarkOliveGreen;

            lightBranchColor = fallLight;
            darkBranchColor = fallDark;
        }
        else if (month >= 3 && month <= 5)
        {
            sun.TranslationY = 75;
            sun.Fill = Color.FromRgb(246, 255, 220);
            nightSky.TranslationY = -75;
            sky.Color = Colors.CornflowerBlue;

            grass.Color = Colors.ForestGreen;

            lightBranchColor = springLight;
            darkBranchColor = springDark;
        }
        else
        {
            sun.TranslationY = 150;
            sun.Fill = Color.FromRgb(255, 245, 153);
            nightSky.TranslationY = 0;
            sky.Color = Colors.SteelBlue;

            grass.Color = Colors.AntiqueWhite;

            lightBranchColor = winterLight;
            darkBranchColor = winterDark;
        }


        branchLight1.Fill = new SolidColorBrush(lightBranchColor);
        branchLight2.Fill = new SolidColorBrush(lightBranchColor);
        branchLight3.Fill = new SolidColorBrush(lightBranchColor);

        branchDark1.Fill = new SolidColorBrush(darkBranchColor);
        branchDark2.Fill = new SolidColorBrush(darkBranchColor);
        branchDark3.Fill = new SolidColorBrush(darkBranchColor);
    }

    private void seasonChanges()
    {
        int month = datePicker.Date.Value.Month;
        uint animationSpeed = (uint)(1000 / stepSpeed.Value);

        if (month >= 6 && month <= 8)
        {
            sun.TranslateTo(0, 0, animationSpeed);
            sun.Fill = Color.FromRgb(255, 220, 0);
            nightSky.TranslateTo(0, -150, animationSpeed);
            sky.Color = Colors.DeepSkyBlue;

            grass.Color = Colors.LawnGreen;

            lightBranchColor = summerLight;
            darkBranchColor = summerDark;
        }
        else if (month >= 9 && month <= 11)
        {
            sun.TranslateTo(0, 75, animationSpeed);
            sun.Fill = Color.FromRgb(237, 175, 79);
            nightSky.TranslateTo(0, -75, animationSpeed);
            sky.Color = Colors.LightSkyBlue;

            grass.Color = Colors.DarkOliveGreen;

            lightBranchColor = fallLight;
            darkBranchColor = fallDark;
        }
        else if (month >= 3 && month <= 5)
        {
            sun.TranslateTo(0, 75, animationSpeed);
            sun.Fill = Colors.LightYellow;
            nightSky.TranslateTo(0, -75, animationSpeed);
            sky.Color = Colors.CornflowerBlue;

            grass.Color = Colors.ForestGreen;

            lightBranchColor = springLight;
            darkBranchColor = springDark;
        }
        else if (month == 12 || month <= 2)
        {
            sun.TranslateTo(0, 150, animationSpeed);
            sun.Fill = Color.FromRgb(255, 245, 153);
            nightSky.TranslateTo(0, 0, animationSpeed);
            sky.Color = Colors.SteelBlue;

            grass.Color = Colors.AntiqueWhite;

            lightBranchColor = winterLight;
            darkBranchColor = winterDark;
        }

        branchLight1.Fill = new SolidColorBrush(lightBranchColor);
        branchLight2.Fill = new SolidColorBrush(lightBranchColor);
        branchLight3.Fill = new SolidColorBrush(lightBranchColor);

        branchDark1.Fill = new SolidColorBrush(darkBranchColor);
        branchDark2.Fill = new SolidColorBrush(darkBranchColor);
        branchDark3.Fill = new SolidColorBrush(darkBranchColor);
    }
    
    private void moonBuild()
    {
        moon = new Ellipse
        {
            Fill = new SolidColorBrush(Colors.WhiteSmoke)
        };

        // Kuu paigutus
        AbsoluteLayout.SetLayoutBounds
            (
                moon,
                new Rect(0.7, 0.3, 75, 75)
            );

        AbsoluteLayout.SetLayoutFlags
            (
                moon,
                AbsoluteLayoutFlags.PositionProportional
           );

        moon.Opacity = 0;
    }

    private async Task nightOpacity(double sekundid)
        //Funktsioon öö varjutuse kontrollimiseks
    {
        uint animationSpeed = (uint)(1000 / stepSpeed.Value);

        double moonOpacity;
        double nightOpacity;

        if (sekundid >= 68400 && sekundid <= 86399)
        {
            moonOpacity = (sekundid - 68400) / 18000.0;
        }

        else if (sekundid >= 0 && sekundid <= 18000)
        {
            moonOpacity = (18000 - sekundid) / 18000.0;
        }
        else
        {
            moonOpacity = 0;
        }

        if (sekundid >= 57600 && sekundid <= 75600)
        {
            nightOpacity = (sekundid - 61200) / 22500.0;
        }
        else if (sekundid >= 10800 && sekundid <= 28800)
        {
            nightOpacity = (25200 - sekundid) / 22500;
        }
        else if (sekundid > 75600 || sekundid < 10800)
        {
            nightOpacity = 1;
        }
        else
        {
            nightOpacity = 0;
        }
        await Task.WhenAll(
            moon.FadeTo(moonOpacity, animationSpeed),
            nightSky.FadeTo(nightOpacity, animationSpeed),
            nightGrass.FadeTo(nightOpacity * 0.8, animationSpeed),
            branchNightD1.FadeTo(nightOpacity * 0.6, animationSpeed),
            branchNightD2.FadeTo(nightOpacity * 0.6, animationSpeed),
            branchNightD3.FadeTo(nightOpacity * 0.6, animationSpeed),
            branchNightL1.FadeTo(nightOpacity * 0.6, animationSpeed),
            branchNightL2.FadeTo(nightOpacity * 0.6, animationSpeed),
            branchNightL3.FadeTo(nightOpacity * 0.6, animationSpeed)
            );
    }

	private async void shakeTree()
    // Värisemise funktsioon
    {
        actionRunning = true;

        uint translateSpeed = (uint)(200 / stepSpeed.Value);
        uint scaleSpeed = (uint)(100 / stepSpeed.Value);
        uint rotateSpeed = (uint)(50 / stepSpeed.Value);

        for (int i = 0; i < 5; i++)
        {
            await Task.WhenAll(
                branchLight1.TranslateTo(5, 5, translateSpeed),
                branchDark1.TranslateTo(-5, -5, translateSpeed),
                branchLight2.TranslateTo(-5, 5, translateSpeed),
                branchDark2.TranslateTo(5, -5, translateSpeed),
                branchLight3.TranslateTo(5, 5, translateSpeed),
                branchDark3.TranslateTo(-5, -5, translateSpeed),

                branchNightL1.TranslateTo(5, 5, translateSpeed),
                branchNightD1.TranslateTo(-5, -5, translateSpeed),
                branchNightL2.TranslateTo(-5, 5, translateSpeed),
                branchNightD2.TranslateTo(5, -5, translateSpeed),
                branchNightL3.TranslateTo(5, 5, translateSpeed),
                branchNightD3.TranslateTo(-5, -5, translateSpeed),

                branchNightL1.ScaleTo(branchNightL1.Scale + 0.05, scaleSpeed),
                branchNightD1.ScaleTo(branchNightD1.Scale - 0.05, scaleSpeed),
                branchNightL2.ScaleTo(branchNightL2.Scale + 0.05, scaleSpeed),
                branchNightD2.ScaleTo(branchNightD2.Scale - 0.05, scaleSpeed),
                branchNightL3.ScaleTo(branchNightL3.Scale + 0.05, scaleSpeed),
                branchNightD3.ScaleTo(branchNightD3.Scale - 0.05, scaleSpeed),

                branchLight1.ScaleTo(branchLight1.Scale + 0.05, scaleSpeed),
                branchDark1.ScaleTo(branchDark1.Scale - 0.05, scaleSpeed),
                branchLight2.ScaleTo(branchLight2.Scale + 0.05, scaleSpeed),
                branchDark2.ScaleTo(branchDark2.Scale - 0.05, scaleSpeed),
                branchLight3.ScaleTo(branchLight3.Scale + 0.05, scaleSpeed),
                branchDark3.ScaleTo(branchDark3.Scale - 0.05, scaleSpeed),

                treeLayout.RotateTo(3, rotateSpeed)
                );
            await Task.WhenAll(
                branchLight1.TranslateTo(-5, -5, translateSpeed),
                branchDark1.TranslateTo(5, 5, translateSpeed),
                branchLight2.TranslateTo(5, -5, translateSpeed),
                branchDark2.TranslateTo(-5, 5, translateSpeed),
                branchLight3.TranslateTo(-5, -5, translateSpeed),
                branchDark3.TranslateTo(5, 5, translateSpeed),

                branchLight1.ScaleTo(branchLight1.Scale - 0.1, scaleSpeed),
                branchDark1.ScaleTo(branchDark1.Scale + 0.1, scaleSpeed),
                branchLight2.ScaleTo(branchLight2.Scale - 0.1, scaleSpeed),
                branchDark2.ScaleTo(branchDark2.Scale + 0.1, scaleSpeed),
                branchLight3.ScaleTo(branchLight3.Scale - 0.1, scaleSpeed),
                branchDark3.ScaleTo(branchDark3.Scale + 0.1, scaleSpeed),

                branchNightL1.TranslateTo(-5, -5, translateSpeed),
                branchNightD1.TranslateTo(5, 5, translateSpeed),
                branchNightL2.TranslateTo(5, -5, translateSpeed),
                branchNightD2.TranslateTo(-5, 5, translateSpeed),
                branchNightL3.TranslateTo(-5, -5, translateSpeed),
                branchNightD3.TranslateTo(5, 5, translateSpeed),

                branchNightL1.ScaleTo(branchNightL1.Scale - 0.1, scaleSpeed),
                branchNightD1.ScaleTo(branchNightD1.Scale + 0.1, scaleSpeed),
                branchNightL2.ScaleTo(branchNightL2.Scale - 0.1, scaleSpeed),
                branchNightD2.ScaleTo(branchNightD2.Scale + 0.1, scaleSpeed),
                branchNightL3.ScaleTo(branchNightL3.Scale - 0.1, scaleSpeed),
                branchNightD3.ScaleTo(branchNightD3.Scale + 0.1, scaleSpeed),

                treeLayout.RotateTo(-3, rotateSpeed)
                );
            await Task.WhenAll(
                branchLight1.TranslateTo(0, 0, translateSpeed),
                branchDark1.TranslateTo(0, 0, translateSpeed),
                branchLight2.TranslateTo(0, 0, translateSpeed),
                branchDark2.TranslateTo(0, 0, translateSpeed),
                branchLight3.TranslateTo(0, 0, translateSpeed),
                branchDark3.TranslateTo(0, 0, translateSpeed),

                branchLight1.ScaleTo(branchLight1.Scale + 0.05, scaleSpeed),
                branchDark1.ScaleTo(branchDark1.Scale - 0.05, scaleSpeed),
                branchLight2.ScaleTo(branchLight2.Scale + 0.05, scaleSpeed),
                branchDark2.ScaleTo(branchDark2.Scale - 0.05, scaleSpeed),
                branchLight3.ScaleTo(branchLight3.Scale + 0.05, scaleSpeed),
                branchDark3.ScaleTo(branchDark3.Scale - 0.05, scaleSpeed),

                branchNightL1.TranslateTo(0, 0, translateSpeed),
                branchNightD1.TranslateTo(0, 0, translateSpeed),
                branchNightL2.TranslateTo(0, 0, translateSpeed),
                branchNightD2.TranslateTo(0, 0, translateSpeed),
                branchNightL3.TranslateTo(0, 0, translateSpeed),
                branchNightD3.TranslateTo(0, 0, translateSpeed),

                branchNightL1.ScaleTo(branchNightL1.Scale + 0.05, scaleSpeed),
                branchNightD1.ScaleTo(branchNightD1.Scale - 0.05, scaleSpeed),
                branchNightL2.ScaleTo(branchNightL2.Scale + 0.05, scaleSpeed),
                branchNightD2.ScaleTo(branchNightD2.Scale - 0.05, scaleSpeed),
                branchNightL3.ScaleTo(branchNightL3.Scale + 0.05, scaleSpeed),
                branchNightD3.ScaleTo(branchNightD3.Scale - 0.05, scaleSpeed),

                treeLayout.RotateTo(0, rotateSpeed)
                );
        }
        statusLabel.Text = "";

        actionRunning = false;
    }

    private async Task bloomTree()
    {
        int month = datePicker.Date.Value.Month;

        if (month < 5 || month > 8)
        {
            statusLabel.Text = "Puu õitseb ainult mai kuni august!";
            return;
        }
        actionRunning = true;
        var animatedBloom = new List<Task>();

        animatedBloom.AddRange(createFlowers(branchLight1));
        animatedBloom.AddRange(createFlowers(branchLight2));
        animatedBloom.AddRange(createFlowers(branchLight3));

        animatedBloom.AddRange(createFlowers(branchDark1));
        animatedBloom.AddRange(createFlowers(branchDark2));
        animatedBloom.AddRange(createFlowers(branchDark3));

        await Task.WhenAll(animatedBloom);

        statusLabel.Text = "";

        actionRunning = false;
    }

    private List<Task> createFlowers(Ellipse branch)
    {
        // Võta oksa asukoht
        Rect branchBounds = AbsoluteLayout.GetLayoutBounds(branch);

        Ellipse flower1 = new Ellipse
        {
            Fill = Colors.Pink,
            Scale = 0
        };

        Ellipse flower2 = new Ellipse
        {
            Fill = Colors.Pink,
            Scale = 0
        };

        AbsoluteLayout.SetLayoutBounds
            (
                flower1,
                new Rect
                (
                    branchBounds.X - 0.05,
                    branchBounds.Y + 0.05,
                    15,
                    15
                )
            );

        AbsoluteLayout.SetLayoutBounds
            (
                flower2,
                new Rect
                (
                    branchBounds.X + 0.05,
                    branchBounds.Y - 0.05,
                    15,
                    15
                )
            );

        AbsoluteLayout.SetLayoutFlags(
            flower1,
            AbsoluteLayoutFlags.PositionProportional
        );

        AbsoluteLayout.SetLayoutFlags(
            flower2,
            AbsoluteLayoutFlags.PositionProportional
        );

        treeLayout.Children.Add(flower1);
        treeLayout.Children.Add(flower2);

        uint scaleSpeed = (uint)(1000 / stepSpeed.Value);

        // kasvata õied
        return new List<Task>
        {
            flower1.ScaleTo(1, scaleSpeed),
            flower2.ScaleTo(1, scaleSpeed)
        };
    }
}
