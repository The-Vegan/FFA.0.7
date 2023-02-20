extends "res://UIAndMenus/DestinationButton/DestinationButton.gd"

func _ready():
	#._ready()
	pass
func _pressed():
	self.destination = mainMenu.postCharacterDestination
	
	mainMenu.CharacterChosen()
	._pressed()
