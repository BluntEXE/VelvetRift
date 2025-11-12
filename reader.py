from tkinter import ttk
import os
import sys
import requests
import json
import subprocess
import atexit
import threading
import time
import tkinter as tk
import random

def resource_path(relative_path):
    """Get absolute path to resource, works for dev and for PyInstaller"""
    base_path = getattr(sys, '_MEIPASS', os.path.abspath("."))
    return os.path.join(base_path, relative_path)
    
# 🔮 Load tarot card data
with open(resource_path("cards.json"), "r", encoding="utf-8") as f:
    cards_data = json.load(f)

# Global storage
last_drawn_cards = []
last_drawn_labels = []
past_line = ""
present_line = ""
future_line = ""
single_line = ""
interpretation_text = ""

# Supported models
available_models = ["mistral", "llama2", "gemma"]
selected_model = "mistral"
ollama_process = None
model_status = "Starting..."

# Launch Ollama model
def launch_model():
    global ollama_process, model_status
    try:
        # Check if model is already running
        check = subprocess.run(["ollama", "list"], capture_output=True, text=True)
        lines = check.stdout.splitlines()
        for line in lines:
            if selected_model in line and "running" in line:
                model_status = "Already running"
                return

        # Launch model
        ollama_process = subprocess.Popen(["ollama", "run", selected_model])
        model_status = "Running"
    except Exception as e:
        model_status = f"Error: {e}"
        print("Model launch error:", e)

def cleanup():
    if ollama_process:
        ollama_process.terminate()

atexit.register(cleanup)

# Load tarot deck
with open(resource_path("cards.json"), "r", encoding="utf-8") as f:
    deck = json.load(f)

# Copy helpers
def copy_to_clipboard(text):
    root.clipboard_clear()
    root.clipboard_append(text.strip())
    root.update()

# AI interpretation
def generate_ai_interpretation(card_name, mode, label, character):
    job = job_var.get()
    job_flavor = f" flavored for a {job}" if job != "None" else ""
    prompt = (
        f"Give a poetic, immersive tarot interpretation for the card '{card_name}', "
        f"in {mode} mode, for a character named '{character}'{job_flavor}. "
        f"Label this as the {label} interpretation. Keep it under 50 words."
    )

    response = requests.post(
        "http://localhost:11434/api/generate",
        json={"model": selected_model, "prompt": prompt, "stream": True},
        stream=True
    )

    full_text = ""
    for line in response.iter_lines():
        if line:
            try:
                chunk = line.decode("utf-8")
                data = json.loads(chunk)
                full_text += data.get("response", "")
                if data.get("done", False):
                    break
            except Exception as e:
                print("Error decoding chunk:", e)

    return full_text.strip()

# RP-style interpretation formatter
def format_interpretation(card, label, mode):
    character = target_entry.get()
    card_name = card["Name"]
    ai_text = generate_ai_interpretation(card_name, mode, label, character)
    return f"/p ✦ {label} Interpretation ({mode}): {ai_text}"

# Single card draw
def draw_single_card():
    global single_line, interpretation_text
    card = random.choice(deck)
    reversed = random.choice([True, False])
    name = card["Name"]
    direction = "Reversed" if reversed else "Upright"
    meaning = card["Meaning"]
    target = target_entry.get()
    mode = mode_var.get()

    single_line = f"/p ✦ Reading for {target}: {name} ({direction}) ✦ Meaning: {meaning}"
    interpretation_text = format_interpretation(card, "General", mode)
    output_box.delete("1.0", tk.END)
    output_box.insert(tk.END, f"{single_line}\n\n{interpretation_text}")


# Three-card spread
def draw_three_card_spread():
    global past_line, present_line, future_line, interpretation_text
    cards = random.sample(deck, 3)
    last_drawn_cards[:] = cards
    last_drawn_labels[:] = ["Past", "Present", "Future"]

    directions = [random.choice([True, False]) for _ in cards]
    target = target_entry.get()
    mode = mode_var.get()

    intro = f"/p ✦ Reading for {target} ✦"
    past_line = f"/p Past: {cards[0]['Name']} ({'Reversed' if directions[0] else 'Upright'}) ✦ Meaning: {cards[0]['Meaning']}"
    present_line = f"/p Present: {cards[1]['Name']} ({'Reversed' if directions[1] else 'Upright'}) ✦ Meaning: {cards[1]['Meaning']}"
    future_line = f"/p Future: {cards[2]['Name']} ({'Reversed' if directions[2] else 'Upright'}) ✦ Meaning: {cards[2]['Meaning']}"

    interpretations = [
        format_interpretation(cards[0], "Past", mode),
        format_interpretation(cards[1], "Present", mode),
        format_interpretation(cards[2], "Future", mode)
    ]

    interpretation_text = "\n".join(interpretations)
    output_box.delete("1.0", tk.END)
    output_box.insert(tk.END, f"{single_line}\n\n{interpretation_text}")


# Regenerate interpretations
def regenerate_interpretations():
    global interpretation_text
    mode = mode_var.get()

    if not last_drawn_cards or not last_drawn_labels:
        output_box.insert("end", "⚠️ No previous draw to regenerate.\n")
        return

    interpretations = [
        format_interpretation(last_drawn_cards[0], last_drawn_labels[0], mode),
        format_interpretation(last_drawn_cards[1], last_drawn_labels[1], mode),
        format_interpretation(last_drawn_cards[2], last_drawn_labels[2], mode)
    ]

    interpretation_text = "\n".join(interpretations)

    output_box.delete("1.0", "end")
    output_box.insert("end", f"{past_line}\n{present_line}\n{future_line}\n\n{interpretation_text}")

# Clear all
def clear_all():
    global past_line, present_line, future_line, single_line, interpretation_text
    past_line = ""
    present_line = ""
    future_line = ""
    single_line = ""
    interpretation_text = ""
    output_box.delete("1.0", "end")  # Clears the visible text area

# UI setup
import tkinter as tk
import ctypes
from tkinter import Frame, Label, Entry, Button, Text, Scrollbar, StringVar, ttk
from PIL import Image, ImageTk
import threading

# Create a hidden root window to register with taskbar
hidden_root = tk.Tk()
hidden_root.withdraw()

# Set AppUserModelID to ensure taskbar icon shows
ctypes.windll.shell32.SetCurrentProcessExplicitAppUserModelID("moonshadow.tarot.reader")

# Create your actual main window as a Toplevel
root = tk.Toplevel()
root.title("Moonshadow's Tarot Reader")
root.geometry("1000x720")
root.minsize(800, 600)
root.overrideredirect(True)
root.resizable(True, True)

# Ensure it behaves like a main window
root.protocol("WM_DELETE_WINDOW", hidden_root.quit)
root.focus_force()
root.lift()
root.attributes("-topmost", True)
root.after(10, lambda: root.attributes("-topmost", False))

# Optional: Set taskbar icon
try:
    root.iconbitmap(resource_path("moonshadow.ico"))
except tk.TclError:
    pass  # Fallback silently if icon fails to load

# 🖱️ Drag-to-move logic
def start_move(event):
    root.x = event.x
    root.y = event.y

def do_move(event):
    x = event.x_root - root.x
    y = event.y_root - root.y
    root.geometry(f"+{x}+{y}")

# 🌙 Global font and widget styles
custom_font = ("Segoe UI", 10)
header_font = ("Haunted Moon", 16, "bold")  # Optional if installed
root.option_add("*Font", custom_font)

button_style = {
    "bg": "#2A1B3D",
    "fg": "#FFFFFF",
    "activebackground": "#B829FE",
    "activeforeground": "#FFFFFF",
    "highlightthickness": 0,
    "bd": 0,
    "relief": "flat"
}

entry_style = {
    "bg": "#333333",
    "fg": "#FFFFFF",
    "insertbackground": "#FFFFFF",
    "relief": "flat"
}

# ✨ Neon combobox style
style = ttk.Style()
style.theme_use("default")
style.configure("Neon.TCombobox",
    fieldbackground="#2A1B3D",
    background="#2A1B3D",
    foreground="#FFFFFF",
    arrowcolor="#B829FE",
    bordercolor="#B829FE",
    relief="flat"
)
style.map("Neon.TCombobox",
    fieldbackground=[("readonly", "#2A1B3D")],
    foreground=[("readonly", "#FFFFFF")],
    background=[("active", "#3A2B4D")],
    arrowcolor=[("active", "#FFFFFF")]
)

# 🖱️ Hover glow effect
def add_hover_effect(widget, glow_color="#B829FE", normal_color="#2A1B3D"):
    widget.bind("<Enter>", lambda e: widget.config(bg=glow_color, cursor="hand2"))
    widget.bind("<Leave>", lambda e: widget.config(bg=normal_color))

# 🌌 Background image
bg_image = Image.open(resource_path("background.png"))
bg_photo = ImageTk.PhotoImage(bg_image)
bg_label = Label(root, image=bg_photo)
bg_label.place(relwidth=1, relheight=1)
bg_label.image = bg_photo

# 🌟 Custom title bar
title_bar = Frame(root, bg="#2A1B3D", relief="raised", bd=0)
title_bar.pack(fill="x")
title_bar.bind("<Button-1>", start_move)
title_bar.bind("<B1-Motion>", do_move)

# 🌙 Title label on the left
title_label = Label(title_bar, text="🌙 Moonshadow's Tarot Reader", bg="#2A1B3D", fg="#FFFFFF", font=("Segoe UI", 10, "bold"))
title_label.pack(side="left", padx=10)

# 🧭 Minimize, Maximize, Close buttons on the right (in correct visual order)
def minimize_window():
    root.iconify()

def toggle_maximize():
    if root.state() == "normal":
        root.state("zoomed")
    else:
        root.state("normal")

# Pack buttons in reverse so they appear left-to-right as: Minimize, Maximize, Close
for text, command in reversed([
    ("—", minimize_window),
    ("⬜", toggle_maximize),
    ("✖", root.destroy)
]):
    btn = Button(title_bar, text=text, command=command, bg="#2A1B3D", fg="#FFFFFF", bd=0, relief="flat")
    btn.pack(side="right", padx=5)
    add_hover_effect(btn)

# 🧙 Container frame
container = Frame(root, bg="#121218")
container.pack(fill="both", expand=True)
bg_label.lower()
container.lift()

# 🔮 Main frame
main_frame = Frame(container, bg="#121218")
main_frame.pack(fill="x", padx=20, pady=(20, 0))

# Left column
left_frame = Frame(main_frame, bg="#121218")
left_frame.pack(side="left", fill="y", expand=True, padx=10, anchor="n")

Label(left_frame, text="Character Name:", bg="#121218", fg="#FFFFFF").pack(anchor="w")
target_entry = Entry(left_frame, **entry_style)
target_entry.pack(pady=5, fill="x")
target_entry.insert(0, "Your Warrior of Light")

Label(left_frame, text="Interpretation Mode:", bg="#121218", fg="#FFFFFF").pack(anchor="w")
mode_var = StringVar()
mode_dropdown = ttk.Combobox(left_frame, textvariable=mode_var, values=["Eorzean", "Chaos", "Maternal"], state="readonly", style="Neon.TCombobox")
mode_dropdown.set("Eorzean")
mode_dropdown.pack(pady=5, fill="x")

Label(left_frame, text="Job Flavor:", bg="#121218", fg="#FFFFFF").pack(anchor="w")
job_var = StringVar()
job_dropdown = ttk.Combobox(left_frame, textvariable=job_var, values=[
    "None", "Paladin", "Warrior", "Dark Knight", "Gunbreaker",
    "White Mage", "Scholar", "Astrologian", "Sage",
    "Monk", "Dragoon", "Ninja", "Samurai", "Reaper",
    "Bard", "Machinist", "Dancer",
    "Black Mage", "Summoner", "Red Mage", "Blue Mage",
    "Viper", "Pictomancer"
], state="readonly", style="Neon.TCombobox")
job_dropdown.set("None")
job_dropdown.pack(pady=5, fill="x")

Label(left_frame, text="Model Selection:", bg="#121218", fg="#FFFFFF").pack(anchor="w")
model_var = StringVar()
model_dropdown = ttk.Combobox(left_frame, textvariable=model_var, values=available_models, state="readonly", style="Neon.TCombobox")
model_dropdown.set("mistral")
model_dropdown.pack(pady=5, fill="x")

selected_model = model_var.get()
threading.Thread(target=launch_model, daemon=True).start()

def update_model_selection(*args):
    global selected_model
    selected_model = model_var.get()
    threading.Thread(target=launch_model, daemon=True).start()

model_var.trace_add("write", update_model_selection)

# Center column
center_frame = Frame(main_frame, bg="#121218")
center_frame.pack(side="left", fill="y", expand=True, padx=10, anchor="n")

Label(center_frame, text="Draw Options:", bg="#121218", fg="#FFFFFF").pack(anchor="w", pady=(0, 5))

for text, command in [
    ("Draw Single Card", draw_single_card),
    ("Draw 3-Card Spread", draw_three_card_spread),
    ("Regenerate Interpretations", regenerate_interpretations),
    ("Clear All", clear_all)
]:
    btn = Button(center_frame, text=text, command=command, **button_style)
    btn.pack(pady=5, fill="x")
    add_hover_effect(btn)

# Right column
right_frame = Frame(main_frame, bg="#121218")
right_frame.pack(side="left", fill="y", expand=True, padx=10, anchor="n")

Label(right_frame, text="Copy to Clipboard:", bg="#121218", fg="#FFFFFF").pack(anchor="w", pady=(0, 5))

for text, source in [
    ("Copy Single Draw", lambda: single_line),
    ("Copy All (3-Card Spread)", lambda: output_box.get("1.0", "end-1c")),
    ("Copy Past", lambda: past_line),
    ("Copy Present", lambda: present_line),
    ("Copy Future", lambda: future_line),
    ("Copy Interpretations Only", lambda: interpretation_text)
]:
    btn = Button(right_frame, text=text, command=lambda s=source: copy_to_clipboard(s()), **button_style)
    btn.pack(pady=2, fill="x")
    add_hover_effect(btn)

# Output frame
output_frame = Frame(container, bg="#121218")
output_frame.pack(fill="both", expand=True, padx=20, pady=(10, 0))

Label(output_frame, text="Interpretation Log", font=("Segoe UI", 12, "bold"), bg="#121218", fg="#B829FE").pack(anchor="w", padx=10, pady=(0, 5))

output_box = Text(output_frame, wrap="word", bg="#1A1A1A", fg="#E0E0E0", relief="flat", font=("Segoe UI", 10), spacing1=4, spacing3=4)
output_box.pack(fill="both", expand=True, padx=10, pady=10)

scrollbar = Scrollbar(output_frame, command=output_box.yview)
scrollbar.pack(side="right", fill="y")
output_box.config(yscrollcommand=scrollbar.set)

# Model status label
status_var = StringVar(value=f"Model Status: {model_status}")
status_label = Label(container, textvariable=status_var, fg="#29FE4A", bg="#121218", anchor="w")
status_label.pack(anchor="w", padx=20, pady=(0, 10))

def refresh_status():
    status_var.set(f"Model Status: {model_status}")
    root.after(1000, refresh_status)

refresh_status()

def copy_to_clipboard(text):
    root.clipboard_clear()
    root.clipboard_append(text.strip())
    root.update()

# Start the app
root.mainloop()
