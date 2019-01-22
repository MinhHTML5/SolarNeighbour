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
	public const float		GRAVITY_CONSTANT 			= 0.00001f;
	
	public const int		MASS_TINY					= 1000;
	public const int		MASS_MEDIUM					= 10000;
	public const int		MASS_LARGE					= 50000;
	
	public const int		POPULATION_TINY				= 5000; // 5 billion
	public const int		POPULATION_MEDIUM			= 7000;
	public const int		POPULATION_LARGE			= 10000;
	
	public const float		RESOURCE_MULTIPLIER			= 0.2f;
	
	
	
	public const float		MISSILE_COST				= 300;
	public const float		MISSILE_SPEED				= 100.0f;
	public const float		MISSILE_LIFE				= 10.0f;
	
	// Client / server variable
	public const float 		PICK_PLANET_TIME 			= 15;
}
