# nmm_game
Dies ist eine Desktop-Anwendung für das Spiel **Mühle** (Nine Men's Morris), welch mit WPF und C# (Visual Studio IDE) entwickelt wurde.

## Regeln
Zwei Spieler bewegen ihre Spielsteine auf einem Spielfeld mit 24 Positionen. Der menschliche Spieler spielt mit roten Spielsteinen, der Computergegner bewegt die blauen Spielsteine. Der menschliche Spieler beginnt das Spiel.
 
Das Spiel besteht aus drei Phasen:
* Phase 1: Die Spieler platzieren abwechselnd ihre Spielsteine auf den freien Positionen des Spielfeldes.
* Phase 2: Wenn ein Spieler am Zug ist, wählt er einen Spielstein aus und bewegt diesen auf eine freie, angrenzende Position. Durch die Bewegung der Spielsteine können gegnerische Spielsteine geschlagen und vom Spielfeld entfernt werden.
* Phase 3: Sobald ein Spieler nur noch drei Spielsteine auf dem Spielfeld übrig hat, verändern sich die Bewegungsregeln: Dieser Spieler darf seine Spielsteine auf jede beliebige, freie Position setzen (fliegen).

Das Spiel endet, wenn einer der Spieler nur noch zwei Spielsteine übrig hat oder wenn er keine Möglichkeiten mehr hat seine Spielsteine legal zu bewegen. 
Tritt keine dieser beiden Bedingungen ein, endet das Spiel nach 70 Spielzügen in einem Unentschieden.

## Implementation
 
Die Anwendung unterstützt folgende Sprachen: Englisch, Deutsch und Französisch. Die Sprachauswahl findet über einen Klick auf den Button mit der entsprechenden Flagge statt.
 
Spielsteine werden graphisch als Ellipsen dargestellt, die entweder rot (menschlicher Spieler) oder blau (Computergegner) gefärbt sind. Freie Positionen sind gelb markiert. 
Wenn eine Ellipse angeklickt wird, löst dies ein Event aus, welches eine der folgenden Aktionen realisiert:
* In Phase 1 des Spiels wird ein roter Spielstein auf eine freie Position platziert.
* In Phase 2 und 3 bedeutet ein Klick auf einen Spielstein, dass dieser Spielstein 
bewegt werden soll, ein weiterer Klick platziert den Spielstein auf einer Position.
* Falls der rote Spieler eine Mühle geformt hat, so kann er mit einem Klick einen blauen Spielstein entfernen.

Wenn ein Spieler eine Mühle geformt hat und einen gegnerischen Spielstein zum Entfernen auswählt, überprüft das Programm ob sich der gegnerische Stein in einer Mühle 
befindet und gibt eine entsprechende Warnung aus. Falls sich alle gegnerischen Spielsteine in einer Mühle befinden sollten, ist es wiederum erlaub sie zu entfernen.
 
Eine *gefährliche Mühle* ist eine unvollständige Mühle, bei der nur noch ein Spielstein fehlt; dies ist in der folgenden Abbildung dargestellt: eine *gefährliche Mühle* wird hier von den Spielsteinen 22 und 24 gebildet.
 
![Dangerous red mill](https://github.com/StefaniaDuma/nmm_game/blob/master/Images/dangerousMill.png)

Ich habe einen Greedy-Algorithmus genutzt, um den Computerspieler zu realisieren. Der Algorithmus geht nach den folgenden Regeln vor:
* Der erste Zug des blauen Spielers ist zufällig.
* Es wird überprüft, ob entweder der rote oder der blaue Spieler dabei sind eine Mühle zu bilden (zwei gleichfarbige angrenzende Spielsteine, die eine gefährliche Mühle bilden).
* Falls es sowohl eine gefährliche rote als auch eine gefährliche blaue Mühle auf dem Spielfeld gibt, wird die blaue Mühle vervollständigt, statt die gegnerische Mühle zu blockieren, da mit der Vervollständigung der eigenen Mühle ein gegnerischer Spielstein zum Spielfeld entfernt werden darf ; hierbei wird ein roter Stein gewählt, der sich in einer gefährlichen (unvollständigen) Mühle befindet.
* Falls es keine gefährliche blaue, aber eine gefährliche rote Mühle auf dem Spielfeld gibt, so wird der blaue Spielstein so gesetzt, dass die gegnerische Mühle blockiert wird.
* Falls es weder eine rote noch eine blaue gefährliche Mühle auf dem Spielfeld gibt, so wird der blaue Spielstein in ein freies, zu einem blauen Spielstein benachbartes, Feld gesetzt um eine gefährliche Mühle aufzubauen.
* Wenn ein roter Spielstein entfernt wird, wird ein Stein ausgewählt, der sich in einer unvollständigen Mühle oder in der Nähe einer solchen befindet; andernfalls wird ein zufälliger roter Spielstein ausgewählt.
 
Der Vorteil eines Greedy-Algorithmus ist seine geringe Laufzeit, der Computergegner ist in der Lage zügig zu agieren. (Ein Timer sorgt für eine Verzögerung, so dass das menschliche Auge die gegnerischen Spielzüge besser nachverfolgen kann.) Der Nachteil dieses Algorithmus ist die manchmal nicht optimale Wahl von Spielzügen.
 
Die ausführbare Datei für das Programm kann im *bin/Debug*-Ordner gefunden werden.
  
 
