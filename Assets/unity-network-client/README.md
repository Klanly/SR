# Action Table

### Services Ping
| Action | Description |Param|Type|Desc|
| :---: | :--- | --- | --- | --- |
| 1 | Returns the payload parameter to the sender. Used mostly to test connectivity with backend services. ||||
||| payload      | Any | Data to be returned |

### Game Logic Ping
| Action | Description |Param|Type|Desc|
| :---: | :--- | --- | --- | --- |
| 1001 | Returns the payload parameter to the sender. Used mostly to test connectivity with the game logic server.  ||||
||| payload | Any | Data to be returned |

### Save State
| Action | Description |Param|Type|Desc|
| :---: | :--- | --- | --- | --- |
| 1002 | Save user's game state to persistent store.  ||||
||| state | Object | This should be a JSON object that is then stringified and stored to the DB as a string. |

### Load State
| Action | Description |Param|Type|Desc|
| :---: | :--- | --- | --- | --- |
| 1003 | Load user's game state from persistent store. ||||
||| Output |||
||| state | Object | Game state loaded from DB. null if it doesn't exist. |

### Skeleton
| Action | Description |Param|Type|Desc|
| :---: | :--- | --- | --- | --- |
| action | description ||||
||| param1 | type | param description |

unity-network-client
====================

A generic implementation for FRAG's AMQP based communication.

Clone this repo into the "Assets" folder of the Unity library, and then make
the following changes

You can instantiate this class in a singleton such as GameManager like so:

```C#
private Networking networkManager;

protected override void Start()
{
	base.Start();
	
	// initialize networking by initiating AMQP connection and login
	networkManager = gameObject.AddComponent<Networking>();

	// ...

}
```
