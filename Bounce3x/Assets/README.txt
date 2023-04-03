KiiWorld-Unity v0.76 (SDK v1.4)
==============================

A simple game to help you get started with Kii SDK on Unity 3D.

Note: if you just want to install Kii Game Cloud support with no demo
game you just need to install Assets/Packs/KiiGameCloud.unitypackage

Files
-----

There are 3 files in the Scripts folder that include Kii Cloud API calls:

1) GameTitle: used to sign-in/sign-up a user and initialize the backend
2) KiiCloudLogin: used to send asynchronous user sign-in/sign-up requests
   to the backend
3) ScoreManager: used to send and receive game high scores from the backend
   for each user
   
Build Settings Order
--------------------

Scenes/GameTitle.unity
Scenes/KiiCloudLogin.unity
Scenes/GameMain.unity
Scenes/GameClear.unity
Scenes/GameOver.unity

Create your own Game Cloud
--------------------------

If you didn't get this project from developer.kii.com with preloaded
app id and key these are the instructions to make it work:

1) Create an account at http://developer.kii.com
2) Create an application as explained in "Register an application"
following steps 1, 2, and 3 (disregard the other sections):
http://documentation.kii.com/en/starts/unity/
Choose Unity as platform for your app and the server location of
your back-end.
3) Write down App Id and App Key assigned to your app as explained in
"Register an application" following step 4 (disregard the other 
sections):
http://documentation.kii.com/en/starts/unity/
4) Set keys from step 3 in your Unity project by choosing one of these
options:
  a) Go to "Kii Game Cloud" editor menu and setup your keys there
  b) Edit file Assets/Plugins/KiiConfig.txt and add your keys there
  c) Replace those keys in file Scripts/KiiAutoInitialize.cs in the
  Kii.Initialize() method directly
5) Run the GameTitle scene

Playing instructions
--------------------

1) Login or register a user
2) Fire the ball with the space key
3) Move the platform with the mouse
4) To sign off a user just sign in with a different user

Want more info?
================

More demos: http://docs.kii.com/en/samples/
Game Cloud Tutorial: http://docs.kii.com/en/samples/Gamecloud-Unity/

Interested in Game Analytics?
-----------------------------

We also offer a dedicated Unity SDK for Game Analytics which you can
download here: http://developer.kii.com/#/sdks
More info: http://documentation.kii.com/en/guides/unity/managing-analytics