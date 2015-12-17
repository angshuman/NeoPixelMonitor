#include <Adafruit_NeoPixel.h>
#ifdef __AVR__
#include <avr/power.h>
#endif

#define MATRIX_SIZE 64
#define PIN 6

#define START 0x01
#define END 0x02

#define POSITION 0x03
#define RED 0x04
#define GREEN 0x05
#define BLUE 0x06
#define OFFSET 0x01

byte red[MATRIX_SIZE];
byte green[MATRIX_SIZE];
byte blue[MATRIX_SIZE];

byte position = 0;
byte command = 0;
byte value1 = 0;
byte value2 = 0;

Adafruit_NeoPixel pixels = Adafruit_NeoPixel(MATRIX_SIZE, PIN, NEO_GRB + NEO_KHZ800);


void setup() {

#if defined (__AVR_ATtiny85__)
  if (F_CPU == 16000000) clock_prescale_set(clock_div_1);
#endif
  // End of trinket special code

  pixels.begin(); // This initializes the NeoPixel library.

  Serial.begin(57600);
}

void loop() {
  byte value;

  if (Serial.available() > 0)
  {
    value = Serial.read();
    self_send(value);
  }
}

void self_send(byte value)
{
  switch (command)
  {
    case 0:
      value1 = value;
      command = 1;
      break;

    case 1:
      value2 = value;
      command = 0;
      state_machine(value1 - OFFSET, value2 - OFFSET);
      break;
  }
}

void state_machine(byte value1, byte value2)
{
  switch (value1)
  {
    case START:
      break;

    case END:
      show_pixels();
      break;

    case POSITION:
      position = value2;
      break;

    case RED:
      red[position] = value2;
      break;

    case BLUE:
      blue[position] = value2;
      break;

    case GREEN:
      green[position] = value2;
      break;
  }
}

void show_pixels()
{
  for (int i = 0; i < MATRIX_SIZE; i++)
  {
    pixels.setPixelColor(i, pixels.Color(red[i], green[i], blue[i]));
  }

  pixels.show();
}
