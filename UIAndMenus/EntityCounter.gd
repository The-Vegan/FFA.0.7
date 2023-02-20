extends Label

var val :int = 12
var mainMenu
func _ready():
	mainMenu = get_parent().get_parent()


func _on_Up_pressed():
	if(val < 16):
		val += 1
		
		if(val < 10):
			self.text = " " + str(val)
		else:
			self.text = str(val)
		
		mainMenu.numberOfEntities = val
		print(mainMenu.numberOfEntities)

func _on_Down_pressed():
	if(val > 4):
		val -= 1
		
		if(val < 10):
			self.text = " " + str(val)
		else:
			self.text = str(val)
		
		mainMenu.numberOfEntities = val
		print(mainMenu.numberOfEntities)


