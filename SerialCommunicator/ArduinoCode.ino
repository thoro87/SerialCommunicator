byte byteRead;

const byte command1 = 1;
const byte command2 = 2;
const byte result1 = 8;
const byte result2 = 9;

void setup() {
  Serial.begin(9600);
}

void loop() {
  if (Serial.available()) {
      byteRead = Serial.read();
      byteRead = byteRead - '0';
      
      switch(byteRead) {
        case command1: Serial.println(result1); break;
        case command2: Serial.println(result2); break;
        default:  break;
      }
  }  
}
