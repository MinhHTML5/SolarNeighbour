using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SCR_Config {
    // Game variable
	public const int		SUN_MASS				 	= 1000000000;
	public const int		SUN_RADIUS				 	= 20;
	public const int		PLANET_RADIUS				= 8;
	public const int		NUMBER_OF_PLANET_TEMPLATE 	= 20;
	public const int		NUMBER_OF_PLANET_CREATE		= 8;
	public const float		ORBIT_DISTANCE_MIN			= 50;
	public const float		ORBIT_DISTANCE_MAX			= 75;
	public const float		GRAVITY_CONSTANT 			= 0.0002f;
	
	public const int		MASS_TINY					= 100000000;
	public const int		MASS_MEDIUM					= 200000000;
	public const int		MASS_LARGE					= 300000000;
	
	public const int		POPULATION_TINY				= 5000; // 5 billion
	public const int		POPULATION_MEDIUM			= 7000;
	public const int		POPULATION_LARGE			= 10000;
	
	public const float		RESOURCE_MULTIPLIER			= 0.2f;
	
	
	
	public const float		MISSILE_COST				= 300;
	public const float		MISSILE_SPEED				= 70.0f;
	public const float		MISSILE_LIFE				= 50.0f;
	public const float		MISSILE_GRAVITY_BOOST		= 3.0f;
	
	// Client / server variable
	public const float 		PICK_PLANET_TIME 			= 7;
}
