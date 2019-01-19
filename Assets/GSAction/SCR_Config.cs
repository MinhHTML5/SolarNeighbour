using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SCR_Config {
    // Game variable
	public const int		SUN_MASS				 	= 1000000000;
	public const int		NUMBER_OF_PLANET_TEMPLATE 	= 20;
	public const int		NUMBER_OF_PLANET_CREATE		= 8;
	public const float		ORBIT_DISTANCE_MIN			= 50;
	public const float		ORBIT_DISTANCE_MAX			= 75;
	public const float		GRAVITY_CONSTANT 			= 0.000001f;
	
	// Client / server variable
	public const float 		PICK_PLANET_TIME 			= 15;
}
