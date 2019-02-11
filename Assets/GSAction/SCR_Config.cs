using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum UpgradeType {
	DAMAGE_1 = 0,
	DAMAGE_2,
	SELF_DESTRUCT,
	SELF_STEERING,
	RELOADER,
	MATERIAL,
	CLUSTER,
	DEFEND,
	REGENERATE,
	AUTOMATE,
	COUNT
}

public class Upgrade {
	public UpgradeType type;
	public float cost;
	public string name;
	public string desc;
}

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
	
	public const float		RESOURCE_MULTIPLIER			= 0.7f;
	
	
	public const int		MISSILE_BASE_PRICE			= 300;
	public const int		MISSILE_UPGRADE_PRICE		= 200;
	public const int		MISSILE_BASE_DAMAGE			= 1500;
	public const int		MISSILE_UPGRADE_DAMAGE_1	= 1000;
	public const int		MISSILE_UPGRADE_DAMAGE_2	= 2500;
	public const float		MISSILE_BASE_COOLDOWN		= 10;
	public const float		MISSILE_UPGRADE_COOLDOWN	= 6;
	public const float		MISSILE_COST				= 300;
	public const float		MISSILE_SPEED				= 70.0f;
	public const float		MISSILE_LIFE				= 30.0f;
	public const float		MISSILE_GRAVITY_BOOST		= 3.0f;
	
	
	public const float		PLANET_REGEN_RATE			= 0.008f;
	
	// Client / server variable
	public const float 		PICK_PLANET_TIME 			= 7;
	
	
	// Upgrade
	public static Upgrade[] upgrades = new Upgrade[(int)UpgradeType.COUNT];
	
	public static void InitUpgrade() {
		for (int i=0; i<(int)UpgradeType.COUNT; i++) {
			upgrades[i] = new Upgrade();
		}
		
		upgrades[0].type = UpgradeType.DAMAGE_1;
		upgrades[0].cost = 3000;
		upgrades[0].name = "Nuclear warhead";
		upgrades[0].desc = "Missile: +1000 population damage.";
		
		upgrades[1].type = UpgradeType.DAMAGE_2;
		upgrades[1].cost = 6000;
		upgrades[1].name = "Anti-matter payload";
		upgrades[1].desc = "Missile: +2500 population damage.";
		
		upgrades[2].type = UpgradeType.SELF_DESTRUCT;
		upgrades[2].cost = 1000;
		upgrades[2].name = "Self-destruct device";
		upgrades[2].desc = "Missile: No longer damage your own planet.";
		
		upgrades[3].type = UpgradeType.SELF_STEERING;
		upgrades[3].cost = 4000;
		upgrades[3].name = "Homing mechanism";
		upgrades[3].desc = "Missile: Actively steer whenever it get closed to a populated planet.";
		
		upgrades[4].type = UpgradeType.RELOADER;
		upgrades[4].cost = 3500;
		upgrades[4].name = "Improved logistic";
		upgrades[4].desc = "Missile: Launch cooldown reduces by 40%.";
		
		upgrades[5].type = UpgradeType.MATERIAL;
		upgrades[5].cost = 1500;
		upgrades[5].name = "Improved material";
		upgrades[5].desc = "Missile: Launch cost reduces by 33%.";
		
		upgrades[6].type = UpgradeType.CLUSTER;
		upgrades[6].cost = 9000;
		upgrades[6].name = "Cluster missile";
		upgrades[6].desc = "Missile: Shoot 3 missiles at slightly different angle at once. Double launch price.";
		
		upgrades[7].type = UpgradeType.DEFEND;
		upgrades[7].cost = 4500;
		upgrades[7].name = "Defense network";
		upgrades[7].desc = "Planet: Reduce incoming population damage by 25%";
		
		upgrades[8].type = UpgradeType.REGENERATE;
		upgrades[8].cost = 4000;
		upgrades[8].name = "Reproduction plan";
		upgrades[8].desc = "Planet: Slowly repopulate if population is not at maximum.";
		
		upgrades[9].type = UpgradeType.AUTOMATE;
		upgrades[9].cost = 7000;
		upgrades[9].name = "Automate gathering";
		upgrades[9].desc = "Planet: Resource gather speed no longer depends on population size.";
	}
}
