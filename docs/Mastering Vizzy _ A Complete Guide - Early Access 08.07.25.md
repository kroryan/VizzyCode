Mastering Vizzy
A Complete Guide


________________






Contents:
1. Introduction
2. The Blocks
2.1. Program Flow
2.2. Operators
2.3. Craft Instructions
2.4. Craft Information
2.5. Events
2.6. Variables
2.7. Lists
2.8. Custom Expressions
2.9. Custom Instructions
3. Multi-function Display
3.1. Introduction
3.2. The Blocks
3.3. Sprites + Special Widgets
4. Creating a Vizzy - Tips
5. FAQ + Common Mistakes
________________


1. Introduction
________________




Vizzy is a programming language used in Juno: New Origins. Similar to Scratch, it exclusively uses blocks to visualize the program. 


In this guide, I'm going to explain how all of these blocks work - on complicated blocks including examples - and how you can create a Vizzy. Additionally, I'll try to cover Multi-function Displays (MfDs) and some common mistakes. 


You can easily find the chapters on the left side (if you're on a computer) or by tapping “More > Document Outline” (if you're on a phone). 


For formatting reasons, I recommend turning on “Print View” in “More”. 
________________




2. The Blocks
________________




In this chapter, I'm going to explain how all blocks work. The subchapters are the same as the categories on the left of the vizzy editor. 


There are multiple types of blocks:


Instructions
Instructions are just the “normal” blocks that tell the craft what to do. Most Vizzy blocks are instructions. 


Loops
Loops are used to run parts of a program multiple types and / or under certain conditions. You can put blocks in and under a loop. Only the blocks in the loop are affected by the loop. In most text-based programming languages (and in the examples in this guide), loops are represented by “{ }”. Everything between these brackets is in the loop. In JNO, loops are blue. 


Operators and Information
These are put into the inputs of other blocks. Operators do calculations and require inputs themselves while information returns information about the flight (e.g. velocity) and doesn't always need input. 


2.1. Program Flow


wait ( 1 ) seconds    Instruction
This block pauses the program by the given time (by default 1 second). Using 0 as input makes the program wait until the next frame. 


wait until (  )    Instruction
Similar to wait ( 1 ) seconds, but the program is paused until the given condition is met. 


Example:


wait until ( activation group ( 1 ) )    “activation group ( 1 )” will be explained later
Set Throttle to ( 1 )    this block will be explained later


This program launches the rocket once activation group 1 is activated. 


repeat ( 10 )    Loop
This block repeats everything that's put into it the given amount of times (by default 10). 


while ( true )    Loop
This block repeats everything that's put into it as long as the condition is met. Using true as input (as it is by default) creates an infinite loop. 


Examples:


while ( activation group ( 1 ) ) {    “activation group ( 1 )” will be explained later
display ( AG 1 is activated! )    This block will be explained later
}


This program puts a message on the screen until AG 1 is deactivated. 


while ( true ) {
display ( time ( time since launch ) )    This line will be explained later
}


This program tells you the time that has passed since the launch. 


for ( i ) from ( 1 ) to ( 10 ) by ( 1 )    Loop
This loop is similar to repeat, but it has more options for customisation. First, i is set to the first given value (by default 1). Then, the actual loop starts. Before each repetition, JNO checks if i is smaller than the second given number (by default 10). If that's the case, everything in this block is executed once. After that, i is increased by the third given value (by default 1). Then, the next repetition starts. 




The parameters:


i is A variable that is deleted once the loop finishes. It is mainly used by JNO to check when the loop ends, but i can also be accessed by the blocks in the loop. 


The first input (by default 1) is the value i is set to when the loop first starts. In most cases (in Vizzy), this value is 1. 


The second input (by default 10) is the value i is compared to. The loop is only repeated while i is smaller than this value. 


The third input (by default 1) is the step size. After each repetition, i is increased by this value. In most cases, this value is 1. 


Example:


for ( i ) from ( 3 ) to ( 30 ) by ( 3 ) {
flight log ( i ) ( false )    “flight log” will be explained later
}


Writes all multiples of three (from 3 to 30) into the flight log. 


if (  ) then    Loop
Everything in this loop is executed once if the given condition is met. If not, this loop is skipped. Example: See “else”


else if (  ) then    Loop
This block can only be used immediately after an if on another else if. Everything in this loop is executed once if the given condition is met and all previous ifs and else ifs weren’t executed because the conditions weren't met. Example: See “else”


else    Loop
This block can only be used immediately After an if or else if. Everything in this loop is executed once if all previous ifs and else ifs weren't executed because the conditions weren't met. 


Example:


if ( altitude ( ASL ) < 10000 ) {    “Altitude ( ASL )”, “Set Throttle” and “<” will be 
Set Throttle to ( 1 )            explained later
}
else if ( altitude ( ASL ) < 30000 ) {
Set Throttle to ( 0.5 )
}
else {
Set Throttle to ( 0 )
}


This program adjusts the throttle depending on the altitude. 


display ( text )    Instruction
Writes the given text on the screen for a few seconds. 


local log ( text )    Instruction
Writes the given text in the local log of the part running the vizzy. 


flight log ( text )    Instruction
Writes the given text in the flight log. 


break    Instruction
Immediately ends a while or repeat loop. 


# ( Comment text )
This block is completely ignored by the program. You can use it to add notes to your program (e.g. to make it easier to understand). 




2.2. Operators


Operators are put into the inputs of other blocks. They always require inputs themselves (which can be information, set values or even other operators). 


( 0 ) + ( 0 )    Operator
Returns the sum of the given numbers. Returns a+b




( 0 ) - ( 0 )    Operator
Subtracts the second number from the first. Returns a-b


( 0 ) / ( 0 )    Operator
Divides the first number by the second. Returns a÷b


( 0 ) * ( 0 )    Operator
Multiplies the two numbers. Returns a×b


( 0 ) ^ ( 0 )    Operator
Raises the first number to the power of the second number. Returns ab


( 0 ) % ( 0 )    Operator
Returns the remainder of the left number divided by the right number. 


random ( 0 ) to ( 0 )    Operator
Returns a random number between the first and the second given number. 


min of ( 0 ) and ( 0 )    Operator
Returns the smaller of the given numbers. 


max of ( 0 ) and ( 0 )    Operator
Returns the higher of the given numbers. 


(  ) and (  )    Operator
Returns true if both sides are true. Otherwise, returns false. 


(  ) or (  )    Operator
Returns true if at least one side is true. If both sides are false, returns false. 


not (  )    Operator
Inverts the given true/false value. If given “true”, returns “false”. Otherwise, returns “true”. 




( 0 ) = ( 0 )    Operator
Returns true if both sides are equal. Otherwise, returns false. 


( 0 ) < ( 0 )    Operator
Returns true if the left number is smaller than the right number. 


( 0 ) <= ( 0 )    Operator
Returns true if the left number is smaller than or equal to the right number. 


( 0 ) > ( 0 )    Operator
Returns true if the left number is higher than the right number. 


( 0 ) >= ( 0 )    Operator
Returns true if the left number is higher than or equal to the right number. 


( abs ) of ( 0 )    Operator
Performs different calculations with a single number. 
abs = returns the same number, but always positive abs(4) = 4, abs(-13.2) = 13.2
floor = returns the highest integer smaller than the number floor(3.14) = 3
ceiling = returns the smallest integer higher than the number ceiling(3.14) = 4
round = returns the nearest integer of the number round(12.42) = 12
sqrt = returns the square root of the number
sin = returns the sine of the number
cos = returns the cosine of the number
tan = returns the tangent of the number
asin = returns the arc-sine of the number (in radians)
acos = returns the arc-cosine of the number (in radians)
atan = returns the arc-tangent of the number (in radians)
ln = returns the natural log of the number
log = returns the log base 10 of the number
deg2rad = converts the number from degrees to radians
rad2deg = converts the number from radians to degrees






atan2 of ( y ) and ( x )    Operator
Returns the angle in radians between the positive x-axis and the ray to the point. 


if (  ) then ( 0 ) else ( 0 )    Operator
If the given condition is met, returns the first number (by default 0). Otherwise, returns the second number (by default 0 too). 


join ( alpha ) ( bravo ) (  )    Operator
Creates a string of all given values (these values can be true/false, strings, vectors and numbers). 


Example:


display ( join ( Current altitude:  ) ( altitude(AGL) ) ( m ) ) altitude will
                                                                                                                     be explained later
This program displays the current altitude in a user-friendly way. 


length of ( alpha )    Operator
Returns the amount of letters in the given string. 


letter ( 1 ) of ( alpha )    Operator
Returns the given letter in the string. 
Note: Unlike in most programming languages, “1” is the first letter. 


letters ( 2 ) to ( 4 ) of ( alpha )    Operator
Similar to “letter ( 1 ) of ( alpha )”, but returns multiple letters starting from the first given number and ending with the second given number. 


( alpha ) contains ( a )    Operator
Returns true if the first given string contains the second given string. 


format ( {0:n1} ) ( 10000.99 ) ( )    Operator
Advanced formatting that uses C#’s built-in System.String method. Refer to Microsoft's documentation for more information. 


friendly ( distance ) ( 0 )    Operator
Formats the value in a user-friendly way. 


vec ( 0 ), ( 0 ), ( 0 )    Operator
Creates a vector from the given numbers. 


( (0, 0, 0) ) ( length )    Operator
Performs calculations with a single vector. 
x = returns the x-value (the first number) of the vector
y = returns the y-value (the second number) of the vector
z = returns the z-value (the third number) of the vector
length = returns the length of the vector
norm = returns a vector with the same direction but a length of 1


( (0, 0, 0) ) ( dot ) ( (0, 0, 0) )    Operator
Performs calculations with two vectors. 
angle = returns the angle between the two vectors (in degrees)
clamp = clamps the magnitude of the left vector to the magnitude of the
              right vector
cross = returns the cross product of the two vectors
dot = returns the dot product of the two vectors
dist = returns the distance between the two vectors
min = returns a vector made of the minimum components of each vector
max = returns a vector made of the maximum components of each vector
project = projects the left vector onto the right vector
scale = multiplies the vectors component-wise


fUNk ( )    Operator
Evaluates an input expression in this part's context. 


true    Operator
Always returns true


false    Operator
Always returns false
________________
2.3. Craft Instructions


activate stage    Instruction
Activates the next stage. 


set ( Throttle ) to ( 0 )    Instruction
Sets the given input to the given value. This value is a number between 0 and 1 (0 = 0%, 1 = 100%). 


set craft ( Pitch ) to ( 0 )    Instruction
Sets the pitch (or heading) of the craft to the given value (in degrees). 


target node ( name )    Instruction
Targets the given planet or craft. 


set activation group ( 1 ) to ( )    Instruction
Activates (if second input true) or deactivates (if second input false) the given activation group. 


lock heading on ( None )    Instruction
Locks the crafts heading (and pitch) on the given direction. 


lock heading on vector ( (0, 0, 0) )    Instruction
Locks the crafts heading (and pitch) on the given vector. 


set time mode to ( Normal )    Instruction
Changes the time mode. Supports paused, slow motion, normal, fast forward and time warp. 


set camera ( Zoom ) to ( 100 )    Instruction
Sets the given property of the user camera to the given value. 


X Rotation = sets x rotation (up-down)
Y Rotation = sets y rotation (left-right)
Tilt = sets tilt
Zoom = sets zoom
Camera Mode = sets camera mode by name
Camera Index = sets camera mode by index
Target offset = offsets the position of the camera target in craft space


set part ( 0 ) ( Activated ) to ( 0 )    Instruction
Sets the given property of the given part (by id) to the given value. 
Activated = If set to true, activates the part. Otherwise, deactivates it. 
Focused = If set to true, focuses the camera on the part. Otherwise,
                  unfocuses it. 
Name = Changes the part's name. 
Explode = Explodes the part with the given amount of power. 1 = Max power
Fuel Transfer = Sets the part's fuel transfer mode. <0 = Drain, 0 = None, >0 = Fill


switch to craft ( 0 )    Instruction
Switches to the craft with the given id. 


beep ( 1000 ) Hz at ( 1 ) volume for ( 2 ) seconds    Instruction
Plays a “Beep” sound with the given frequency (in Hz, first parameter) and the given volume (2nd parameter) and time (3rd parameter). 


frequency of ( C# ) octave ( 3 )    Operator
Returns the frequency of the given note. 




2.4. Craft Information


altitude ( AGL )    Information
Returns the craft's current altitude (in meters). 
AGL = Above Ground Level Distance to the ground
ASL = Above Sea Level Distance to sea level
ASF = Above Sea Floor Distance to sea floor, same as AGL when not over water


orbit ( Apoapsis )    Information
Returns orbit data. 
Apoapsis = Highest point
Periapsis = Lowest point
Time to... = Time until the craft reaches the specified point
Eccentricity = Difference between Periapsis and Apoapsis always from 0 to 1
Inclination = The “tilt” of the orbit (in radians)
Period = The time it takes the craft for one revolution around the planet


atmosphere ( Air Density )    Information
Returns information about the atmosphere at the craft's current position. 


Units:
Air Density - kg/m3
Air Pressure - Pa
Speed of Sound - m/s
Temperature - Kelvin


performance ( Mass )    Information
Even more information!
Engine Thrust = The thrust currently generated by the engines (N)
Mass = The crafts current mass (kg)
Dry Mass = The crafts mass without fuel (kg)
Fuel Mass = The current mass of the fuel (kg)
Max Engine Thrust = The thrust that could be achieved if all engines
                                   were running at 100% (kg)
TWR = The ratio from Thrust to Weight Has to be >1 for liftoff
Current Isp = The crafts current specific impulse (s)
Stage Delta-V = The remaining delta-v in the current stage (m/s)
Stage Burn Time = The remaining burn time in the current stage (s)


fuel ( Stage )    Information
The percentage of fuel remaining. 
Battery = only battery
Stage = only current stage
Mono = only monopropellant
AllStages = entire craft


velocity ( Orbit )    Information
The craft's current velocity. 
Surface = Velocity relative to the current planet's surface [m/s]
Orbit = Velocity relative to the current planet [m/s]
Target = Velocity relative to the current target [m/s]
Gravity = The gravity at the craft's current position [m/s2]
Drag = Current drag [m/s2]
Acceleration = Current acceleration [m/s2]
Angular = Current angular velocity [rad/s]
Lateral = Velocity without vertical movement [ms/]
Vertical = How fast the craft is moving upwards [m/s]
Mach Number = Velocity divided by the speed of sound
Note: Surface, Orbit, Target, Gravity, Drag, Acceleration and Angular return vectors. To get a number, use “(velocity)length” (see 2.2 Operators). 


input ( Throttle )    Information
Returns the specified input as a number from 0 to 1. 
Example: If Slider 1 = 85%, input(Slider 1) returns 0.85


misc ( Grounded )    Information
Returns the specified property. 
Stage = The current stage
Num Stages = The amount of stages
Grounded = Returns true, if the craft is grounded (otherwise: false)
Solar Radiation = The amount of solar radiation the craft is recieving
User Camera Position = The user camera's position
User Camera Pointing = The user camera's direction
User Camera Up Direction = The user camera's upwards pointing
Pitch PIDs = The PID values for the pitch axis
Roll PIDs = The PID values for the roll axis


time ( Time Since Launch )    Information
Returns the specified time (in seconds). 
Frame Delta Time = The time that has passed since the last frame of
                                 execution
Time Since Launch = The time that has passed since launch
Total Time = The in-game time that has passed since the game was
                     created
Warp Amount = The current amount of time warp
Real Time = The real time that has passed since the game was started


name of ( Craft )    Information
The specified name. 
Craft = The current craft
Planet = The craft's current parent (planet/moon/star)
Target Name = The current target
Target Planet = The current target's parent (planet/moon/star)


activation group ( 1 )    Information
Returns true if the specified AG (activation group) is activated. Otherwise, returns false. 


convert ( (0, 0, 0) ) to lat/long/AGL    Operator
Converts the given position (PCI vector) to a latitude/longtitude/altitude (above ground level) vector. 


convert ( (0, 0, 0) ) to lat/long/ASL    Operator
Converts the given position (PCI vector) to a latitude/longtitude/altitude (above sea level) vector. 


convert ( (0, 0, 0) ) to position    Operator
Converts the given lat/long/AGL vector to a position (PCI) vector. 


convert ( (0, 0, 0) ) to position over sea    Operator
Converts the given lat/long/ASL vector to a position (PCI) vector. 


get terrain ( Height ) at lat/long ( (0, 0, 0) )    Information
Returns the specified terrain property at the given latitude and longtitude. 
Color = Terrain's color
Height = Terrain's height above sea level


cast a ray from ( (0, 0, 0) ) towards ( (0, 0, 0) )    Information
Casts an imaginary ray from the given position (PCI) vector towards the given direction. 


planet ( Droo ) ( Radius )    Information
Returns the specified property of the given planet. 
Mass = Mass of the planet (kg)
Radius = Radius (= half diameter) of the planet (m)
Atmosphere Height = ASL altitude of the atmosphere's end
SOI Radius = Maximum ASL where an orbit is possible
Solar Position = Position of the planet (in PCI coordinates)
Child Planets = List of the planet's moons
Crafts = List of all crafts in the planet's SOI (=Sphere of influence)
Craft IDs = List of the IDs of all crafts in the planet's SOI
Parent = Planet's parent (e.g. Luna's parent = Droo)
Structures = List of all structures on the planet
Length of Day = Length of a day on the planet (s)
Length of Year = Length of a year on the planet (s)
Velocity = Planet's velocity as vector (m/s)
Orbit Apoapsis = Apoapsis of the planet's orbit around its parent (m)
Orbit Periapsis = Periapsis of the planet's orbit around its parent (m)
Orbit Period = Time it takes the planet for one revolution around its
                       parent (s)
Orbit Apoapsis Time = Time left until the planet reaches its apoapsis (s)
Orbit Periapsis Time = Time left until the planet reaches its periapsis (s)
Orbit Inclination = Inclination of the planet's orbit (Rad)
Orbit Eccentricity = Eccentricity of the planet's orbit
Note: For more information about specific parameters, refer to “orbit ( Apoapsis )”. The parameters are mostly the same, it's just the planet instead of the craft. 


part ( 0 ) ( Activated )    Information
Returns the specified property of the part with the given ID. 
Name = Part's name
Mass = Part's mass (kg)
Dry Mass = Part's mass without fuel (kg)
Wet Mass = Part's wet mass (kg)
Activated = True if the part is activated, otherwise false
Part Type = Type of the part (e.g. “fuel tank”)
Position = Part's position (in PCI coordinates)
Temperature = Part's temperature (Kelvin)
Drag = Drag the part is generating (N) Estimation
This Part ID = ID of the part that's running the vizzy
Min Part ID = Smallest part ID in the craft
Max Part ID = Highest part ID in the craft
Under Water = How much of the part is under water (0 = nothing, 1 = all)


part ID of ( PartName )    Information
Returns the ID of the part with the given name. 


part ( 0 ) ( Local to PCI ) ( (0, 0, 0) )    Information
Converts the given coordinates. 
Local to PCI = Converts from the part's local space to PCI coordinates (if
                        no part is specified, converts from craft's local space)
PCI to Local = Converts from PCI coordinates to the part's local space (if
                        no part if specified, converts to craft's local space)


craft ( 0 ) ( Position )    Information
Returns the specified information about the given craft. 
Altitude = Crafts ASL altitude (m)
Destroyed = True if the craft is destroyed, otherwise false
Grounded = True if the craft is on the ground, otherwise false
Mass = Craft's mass (kg)
Name = Craft's name
Part Count = Craft's part count
Planet = Craft's planet (= parent)
Position = Craft's position (in PCI coordinates)
Velocity = Craft's velocity vector (m/s)
Is Player = True if currently controlled by the player, otherwise false
Bounding Box Min = Distance of the min corner of the bounding box to
                                 the Center of Mass (m)
Bounding Box Max = Distance of the max corner of the bounding Box to
                                  the Center of Mass (m)
Orbit Apoapsis = Apoapsis of the craft's orbit (m)
Orbit Periapsis = Periapsis of the craft's orbit (m)
Orbit Period = Time a full revolution around the craft's planet takes (s)
Orbit ApoapsisTime = Time until the craft reaches its apoapsis (s)
Orbit PeriapsisTime = Time until the craft reaches its periapsis (s)
Orbit Inclination = Inclination of the craft's orbit
Orbit Eccentricity = Eccentricity of the craft's orbit
Orbit Mean Anomaly = Mean Anomaly of the craft's orbit
Orbit Mean Motion = Mean Motion of the craft's orbit
Orbit Periapsis Argument = Argument of Periapsis of the craft's orbit
Orbit Right Ascension = Right Ascension of the craft's orbit
Orbit True Anomaly = True Anomaly of the craft's orbit
Orbit Semi Major Axis = Semi Major Axis of the craft's orbit
Orbit Semi Minor Axis = Semi Minor Axis of the craft's orbit
Note: For more information about specific parameters, refer to “orbit ( Apoapsis )”


craft ID of ( CraftName )    Information
ID of the craft with the specified name. 


true    Operator
Always returns true. 


false    Operator
Always returns false. 




2.5. Events


Events are the blocks that start a program (except for “broadcast ( message ) with data ( 0 )” and its variations). For a vizzy to work, you always need an event block in front. You decide for an event block based on when the vizzy should start. The more complex event blocks additionally return values that you can use like variables within the program the block starts. 


on start    Event
Starts the vizzy once the craft is launched. The most common event block. 


on ( part ) collide with ( other ) at ( velocity ) and ( impulse )    Event
Starts the vizzy every time (at least) two parts collide. Additionally returns the part's names, velocity and impulse. 


on ( part ) exploded    Event
Starts the vizzy every time a part explodes. Additionally returns its name. 
on ( craftA ) and ( craftB ) docked    Event
Starts the vizzy every time the craft docks with another craft. Additionally returns both craft's names. 


on enter ( planet ) SOI    Event
Starts the vizzy every time the craft enters any planet's (or moon's) SOI (= Sphere of Influence). Additionally returns that planet's (or moon's) name. 


receive ( message ) with ( data )    Event
Starts the vizzy every time a “broadcast ( message ) with data ( 0 )” (or its variations) with the same message input is executed. Additionally returns the data that's sent. 
Note: For a better understanding, I recommend also reading the “broadcast ( message ) with data ( 0 )” explanation. 


broadcast ( message ) with data ( 0 )    Event
Broadcasts the given message to the entire vizzy. If there is a “receive ( message ) with data ( 0 )” block with the same message in the vizzy, that part of the vizzy is started. You can additionally send data using the data input. 
Note: For a better understanding, I recommend also reading the “receive ( message ) with ( data )” explanation.


broadcast ( message ) with data ( 0 ) to craft    Event
Variation of the “broadcast ( message ) with data ( 0 )” block that broadcasts the message to all vizzys in the craft. 


broadcast ( message ) with data ( 0 ) to nearby crafts    Event
Variation of the “broadcast ( message ) with data ( 0 )” block that broadcasts the message to all crafts in the physics range. 




2.6. Variables


Variables can be used to save data. Before using them, you first have to create a variable using the blue button in the bottom. You can then use the green block that will be created as input for other blocks or save values to the variable using the blocks explained below. 


set variable (  ) to ( 0 )    Instruction
Sets the specified variable to the given value. 


change variable (  ) by ( 0 )    Instruction
Adds the given value (negative values for subtraction) to the specified variable. 


set variable (  ) to user input ( Message text… )    Instruction
Prompts the player to input a value. The variable is then set to that value. 




2.7. Lists


Lists are similar to variables, but you can store multiple values at the same time in a single list. The positions of the values are numbered with whole numbers, starting from 1. 


add ( item ) to (  )    Instruction
Adds the given item to the end of the specified list. 


insert ( item ) at ( 1 ) in (  )    Instruction
Inserts the given item at the given position in the specified list. 


remove ( 1 ) from (  )    Instruction
Removes the value at the given position from the specified list. 


set ( 1 ) in (  ) to ( item )    Instruction
Overwrites the value at the given position in the specified list. 


remove all from (  )    Instruction
Removes all items from the specified list. 


sort (  )    Instruction
Sorts the list (Low values -> High values). 


reverse (  )    Instruction
Reverses the list. 


set list (  ) to ( create list from ( a, b, c ) )    Instruction
Removes all items from the list, then adds the given values to the list. 


item ( 1 ) in (  )    Information
Returns the item at the given position of the specified list. 


length of (  )    Information
Returns the amount of items in the specified list. 


index of ( item ) in list (  )    Information
Returns the position where the item first occurs in the specified list. 




2.8. Custom Expressions


Custom expressions allow you to create your own operators. To create a custom expression, first click on the blue button in the bottom. Then, enter a name and, if necessary, add parameters. Finally, create the expression and find it in the vizzy. There, you can add operators and information blocks to create the expression. 


Example:


A Custom Expression that rounds any number x to any number of decimals n. 
1. The formula: round( x * (10 ^ n) ) / (10 ^ n)
2. Creating the custom expression:  




2.9. Custom Instructions


Custom instructions are similar to custom expressions, but they run code instead of returning values. You can create a custom instruction like custom expressions. 
________________
3. Multi-function Display
________________




3.1. Introduction


Multi-function Displays (MfDs) are customizable screens. They use widgets to build a custom interface. To make an MfD custom, set “MFD Program” in “Multi-function Display” (Part Properties) to “Custom”. 
MfD coordinates (e.g. position, size) are specified using the x and y axis of a vector (with y being height). 


3.2. The Blocks


create ( Label ) widget named ( name )    Instruction
Creates a widget with the given type and name. 
Types: Ellipse, Label, Line, Radial Gauge, Rectangle, Texture, Navball,
            Map


set widget ( name ) ( position ) to (  )    Instruction
Sets the specified property of the widget with the given name. 
Anchored Position = Position relative to its anchor point within its parent
Anchor Min = Normalized position (0-1) in the parent widget that the
                      lower left corner of this widget is anchored to
Anchor Max = Normalized position (0-1) in the parent widget that the
                       upper right corner of this widget is anchored to
Color = Color of the widget as vector in RGB (Red, Green, Blue) scheme
             (values from 0 to 1)
Opacity = Opacity from 0 (transparent) to 1 (opaque)
Parent = The widget’s parent
Pivot = The point on the widget it rotates around (in normalized position
            [0 to 1])
Position = Position (in pixels) relative to the screen’s center
Rotation = Widget’s rotation in degrees
Scale = Widget’s scale as percentage from 0 to 1
Size = Widget’s size in pixels
Visible = Widget’s visibility (true = visible, false = invisible)


set widget ( name ) anchor to ( Center )    Instruction
Sets where the widget is anchored to the parent. 
Positions: Left, Center, Right, TopLeft, TopCenter, TopRight, BottomLeft,
                BottomCenter, BottomRight


set label ( name ) ( Text ) to (  )    Instruction
Sets the specified property of the given label. 
Text = The text the label shows
Font Size = Size o the label’s font
Auto Size = If set to true, automatically sets the label’s font size to the
                   highest possible


set label ( name ) alignment to ( Center )    Instruction
Sets where the text on the label is aligned. 
Alignments: Left, Center, Right, TopLeft, TopCenter, TopRight,
                    BottomLeft, BottomCenter, BottomRight


initialize texture ( name ) width ( 0 ) and height ( 0 )    Instruction
Initializes the given texture with the specified width and height (in pixels). 


set texture ( name ) pixel at x=( 1 ), y=( 1 ) to ( (0, 0, 0) )    Instruction
Sets the pixel at the given position of the specified texture to the given color (as RGB vector, each value ranching from 0 to 1). 


set sprite ( name ) ( Fill Amount ) to (  )    Instruction
Sets the sprite's given property to the specified value. 
Fill Method = Method used to fill the sprite (Options: none, horizontal,
                      vertical, radial90, radial180, radial360)
Icon = Icon shown on the sprite (Sprite Guide)
Fill Amount = Amount the sprite is filled (0 to 1)


set gauge ( name ) ( Value ) to (  )    Instruction
Sets the gauge’s specified property to the given value. 
Background Color = Color of the gauge's background
Color = Color of the gauge
Text = Gauge's labeling
Text Color = Color of the gauge's labeling
Value = How much of the gauge is filled (0 to 1)


set line ( name ) ( Thickness ) to ( 0.5 )    Instruction
Sets the line’s specified property to the given value. 
Thickness = Line's thickness
Length = Line's length


set line ( name ) from ( 0,0 ) to ( 1,1 )    Instruction
Sets the coordinates the given line starts and ends at. 


set navball ( name ) ( Top Color ) to (  )    Instruction
Sets the navball’s specified property to the given value. 
Top Color = Color of the navball's upper half (default: blue)
Bottom Color = Color of the navball's lower half (default: red)


set map ( name ) ( North Up ) to ( true )    Instruction
Sets the map’s specified property to the given value. 
North Up = If true, north faces up, if false, follows craft's heading
                   (overridden by Manual Mode)
Zoom = Zoom level (1 to 5)
Manual Mode = If true, will listen to the other inputs of this block instead
                          of following the craft
Planet = Planet shown on the map
Coordinates = Coordinates of the map's center (lat/long)
Heading = Heading that faces up on the map


bring widget ( name ) in front of ( target )    Instruction
Renders the specified widget in front of the specified target widget. 


send widget ( name ) behind ( target )    Instruction
Renders the specified widget behind the specified target widget. 


set widget ( name ) to broadcast ( message ) on ( Click ) with data ( data )    Instruction
Makes the widget broadcast the specified message every time the given event happens (by default: Click). If there is a “receive ( message ) with data ( 0 )” block with the same message in the vizzy, that part of the vizzy is started. You can additionally send data using the data input. 
Note: For a better understanding, I recommend also reading the “broadcast ( message ) with data ( 0 )” and “receive ( message ) with ( data )” explanations.
Drag = User drags the cursor across the widget
Pointer Down = User presses down on the widget
Pointer Up = User releases their pointer on the widget
Click = User clicks on the widget


destroy widget named ( name )    Information
Deletes the specified widget. 


destroy all widgets    Instruction
Deletes all widgets. 


get widget ( name ) ( Position )    Information
Returns the specified property of the given widget. 
Anchored Position = Position relative to its anchor point within its parent
Anchor Min = Normalized position (0-1) in the parent widget that the
                      lower left corner of this widget is anchored to
Anchor Max = Normalized position (0-1) in the parent widget that the
                       upper right corner of this widget is anchored to
Color = Color of the widget as vector in RGB (Red, Green, Blue) scheme
             (values from 0 to 1)
Exists = true if the widget exists, otherwise false
Opacity = Opacity from 0 (transparent) to 1 (opaque)
Parent = The widget’s parent
Pivot = The point on the widget it rotates around (in normalized position
            [0 to 1])
Position = Position (in pixels) relative to the screen’s center
Rotation = Widget’s rotation in degrees
Scale = Widget’s scale as percentage from 0 to 1
Size = Widget’s size in pixels
Visible = Widget’s visibility (true = visible, false = invisible)


get label ( name ) ( Text )    Information
Returns the specified property of the given label. 
Text = The text the label shows
Font Size = Size o the label’s font
Auto Size = If set to true, automatically sets the label’s font size to the
                   highest possible
Alignment = Where the text on the label is aligned


get sprite ( name ) ( Fill Amount )    Information
Returns the specified property of the given sprite. 
Fill Method = Method used to fill the sprite (Options: none, horizontal,
                      vertical, radial90, radial180, radial360)
Icon = Icon shown on the sprite (Sprite Guide)
Fill Amount = Amount the sprite is filled (0 to 1)


get gauge ( name ) ( Value )    Information
Returns the specified property of the given gauge. 
Background Color = Color of the gauge's background
Color = Color of the gauge
Text = Gauge's labeling
Text Color = Color of the gauge's labeling
Value = How much of the gauge is filled (0 to 1)


get texture ( name ) pixel at x=( 1 ), y=( 1 )    Information
Returns the color of the specified texture's given pixel. 


convert ( 0,0 ) ( Local to Display ) for ( name )    Operator
Converts the given coordinate between display coordinates (= display) and coordinates on the widget (= local). 
Local to Display = From the widget's local position to the display’s global
                             position
Display to Local = From the display's global position to the widget's local
                             position


get widget ( name ) ( Click ) message    Information
Returns the message that is broadcast on the specified event. 
Note: For a better understanding, I recommend also reading the “set widget ( name ) to broadcast ( message ) on ( Click ) with data ( data )” and “receive ( message ) with ( data )” explanations.
Drag = User drags the cursor across the widget
Pointer Down = User presses down on the widget
Pointer Up = User releases their pointer on the widget
Click = User clicks on the widget


hex color ( #FF0000 )    Information
Converts the given HEX color to an RGB color. 
________________
4. Creating a Vizzy - Tips
________________




At first, programming might seem difficult. How can some text make a rocket fly? How can you make a game with just some text? I can't make step-by-step instructions for every possible Vizzy, but I can help you learn to answer those questions yourself. 


First of all, your first project:
As a first project, I recommend making a simple orbit Vizzy for a simple rocket. Why? Because you should choose a simple program where you can exactly replicate what the player does, without any complicated calculations. Also, I recommend using an SSTO (Single Stage to Orbit, e.g. a SpaceX Starship without booster) so you don't need to make a staging program. 


Project chosen? Great, let's get started! But… how? Vizzy can only follow simple instructions (the ones explained above), so you'll need to break down your project into small steps. 


Example:


A simple orbit
Requirements: 
* Simple Gravity turn: Pitch to 45° at 8km
* Apoapsis and Periapsis above 100km
Then, manually do what the vizzy should do and track what you're doing:
1. Lock Heading => lock heading on ( Current )
2. Activate the Engines => activate stage
3. Launch => set ( Throttle ) to ( 1 )
4. Wait for Gravity Turn => wait until ( altitude ( ASL ) > 8000 )
5. Gravity Turn => set craft ( Pitch ) to ( 45 )
6. Wait until the apoapsis is high enough 
=> wait until ( orbit ( apoapsis ) > 120000 )
7. Turn off the engines => set ( Throttle ) to ( 1 )
8. Heading lock => lock heading on ( Prograde )
9. Wait for orbit insertion => wait until ( orbit ( time to apoapsis ) < 30 )
10. Orbit insertion => set ( Throttle ) to ( 1 )
11. Wait until periapsis is high enough 
     => wait until ( orbit ( periapsis ) > 100000 )
12. Turn off engines => set ( Throttle ) to ( 0 )
Finally, exactly replicate these steps using Vizzy. If you tracked your flight correctly (like in the example above), you can just convert each action into a Vizzy block. 


Breaking down a flight into such small steps might feel overcomplicated at first, but after a while you'll get used to it.