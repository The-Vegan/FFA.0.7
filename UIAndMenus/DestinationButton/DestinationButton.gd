extends Button

onready var mainMenu = get_parent().get_parent()
var destination : int

signal gamemode

const BACK = -1
const MAINMENU = 0
const SOLO = 1
const CHARACTERSELECT = 2
const LEVELSELECT = 3
const MULTI = 4

var mode = -1

func _ready():
	$AnimatedSprite.play("Normal")
	connect("gamemode",mainMenu,"SetGame")

#YOU NEED TO OVERRIDE THE DESINATION IN THE READY OVERRIDE
func _pressed():
	mainMenu.MoveCameraTo(destination)
	if (mode >= 0):
		emit_signal("gamemode",mode)

func _notification(what):
	match(what):
		NOTIFICATION_MOUSE_ENTER:
			$AnimatedSprite.play("Hovered")
		NOTIFICATION_MOUSE_EXIT:
			$AnimatedSprite.play("Normal")

