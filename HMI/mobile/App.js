import { useEffect, useState } from 'react';
import { StyleSheet, Text, View, Button, ActivityIndicator, ScrollView } from 'react-native';
import * as SignalR from '@microsoft/signalr';

// ⚠️ REMPLACE PAR L'ADRESSE IPv4 DE TON PC
const IP_ADDRESS = "192.168.1.14"; 
const API_URL = `http://${IP_ADDRESS}:5000/hubs/theater`;
const FETCH_URL = `http://${IP_ADDRESS}:5000/api/play/moliere`;
const ROOM_ID = "SALLE_MOLIERE";

export default function App() {
  const [connection, setConnection] = useState(null);
  const [currentLineIndex, setCurrentLineIndex] = useState(0);
  const [isConnected, setIsConnected] = useState(false);
  const [playData, setPlayData] = useState(null); // Pour stocker le script JSON

  // 1. Initialisation
  useEffect(() => {
    // Récupérer le script JSON
    fetch(FETCH_URL)
      .then(response => response.json())
      .then(data => setPlayData(data))
      .catch(err => console.error("Erreur de chargement du script:", err));

    // Préparer SignalR
    const newConnection = new SignalR.HubConnectionBuilder()
      .withUrl(API_URL)
      .withAutomaticReconnect()
      .build();

    setConnection(newConnection);
  }, []);

  // 2. Connexion WebSockets
  useEffect(() => {
    if (connection) {
      connection.start()
        .then(() => {
          setIsConnected(true);
          connection.invoke("JoinRoom", ROOM_ID);
          connection.on("ReceiveLineUpdate", (lineIndex) => {
            setCurrentLineIndex(lineIndex);
          });
        })
        .catch(e => console.log("Erreur SignalR : ", e));
    }
  }, [connection]);

  const handleNextLine = async () => {
    if (connection) {
      await connection.invoke("NextLine", ROOM_ID);
    }
  };

  // Sécurité pendant le chargement
  if (!playData || !isConnected) {
    return (
      <View style={styles.container}>
        <ActivityIndicator size="large" color="#0000ff" />
        <Text>Chargement de la scène...</Text>
      </View>
    );
  }

  // Récupérer la réplique actuelle depuis le JSON
  const currentLine = playData.script[currentLineIndex];

  return (
    <View style={styles.container}>
      <Text style={styles.title}>{playData.metadata.title}</Text>
      <Text style={styles.room}>Salle : {ROOM_ID}</Text>

      <ScrollView contentContainerStyle={styles.scriptArea}>
        {/* On gère l'affichage selon le type (Header ou Dialogue) */}
        {currentLine.type === "header" ? (
          <Text style={styles.headerText}>{currentLine.content}</Text>
        ) : (
          <>
            <Text style={styles.characterName}>{currentLine.character}</Text>
            {currentLine.didascaly && (
              <Text style={styles.didascalyText}>({currentLine.didascaly})</Text>
            )}
            <Text style={styles.dialogueText}>{currentLine.content}</Text>
          </>
        )}
      </ScrollView>
          
      <View style={styles.footer}>
        <Button 
          title="Réplique Suivante ➡️" 
          onPress={handleNextLine} 
          color="#2ecc71"
        />
      </View>
    </View>
  );
}

const styles = StyleSheet.create({
  container: { flex: 1, backgroundColor: '#fff', paddingTop: 60 },
  title: { fontSize: 24, fontWeight: 'bold', textAlign: 'center' },
  room: { fontSize: 14, color: 'gray', textAlign: 'center', marginBottom: 20 },
  scriptArea: { paddingHorizontal: 20, flexGrow: 1, justifyContent: 'center' },
  headerText: { fontSize: 20, fontWeight: 'bold', textAlign: 'center', color: '#8e44ad' },
  characterName: { fontSize: 22, fontWeight: 'bold', color: '#2c3e50', textAlign: 'center' },
  didascalyText: { fontSize: 16, fontStyle: 'italic', color: '#7f8c8d', textAlign: 'center', marginBottom: 10 },
  dialogueText: { fontSize: 18, lineHeight: 28, textAlign: 'center', marginTop: 10 },
  footer: { padding: 30, paddingBottom: 50, backgroundColor: '#f8f9fa' }
});