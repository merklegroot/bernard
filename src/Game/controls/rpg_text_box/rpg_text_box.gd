extends Control

const LABEL_PATH = "Control#TextLabel"
const BEEP_SOUND_PATH = "res://assets/text-box-beep.wav"

const CHAR_PER_SECOND = 100

@onready var _label = get_node(LABEL_PATH)

var _full_text: String = ""
var _current_text: String = ""
var _char_index: int = 0
var _text_start_time: int = -1
var _beep_sound: Resource = null

@export var text: String = "Some text for the game":
	set(value):
		_full_text = value
		_current_text = ""
		_char_index = 0
		_text_start_time = Time.get_ticks_msec()
		_set_display_text("")

	get:
		return _full_text

func _set_display_text(value: String):
	if _label == null:
		return

	if _label.text == value:
		return

	if value.length() > _label.text.length():
		var new_text = value.substr(_label.text.length())
		var prev_char = _label.text.substr(_label.text.length() - 1, 1) if _label.text.length() > 0 else " "
		
		# Beep when transitioning from space/empty to non-space (start of word)
		if prev_char.strip_edges() == "" and new_text.strip_edges() != "":
			_play_beep()

	_label.text = value

func _play_beep():
	var audio_player = AudioStreamPlayer.new()
	add_child(audio_player)
	
	audio_player.stream = _beep_sound
	audio_player.play()
	
	await audio_player.finished
	audio_player.queue_free()

func _ready():
	_beep_sound = load(BEEP_SOUND_PATH)
	pass

func _process(_delta):
	_set_display_text(_full_text.substr(0, get_chars_to_reveal()))

func get_chars_to_reveal() -> int:
	if _text_start_time == null:
		return 0

	var elapsed = Time.get_ticks_msec() - _text_start_time
	return int(elapsed / (1000.0 / CHAR_PER_SECOND))
