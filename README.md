# Ανάκτηση Πληροφορίας στον Παγκόσμιο Ιστό - Απαλλακτική Εργασία - Documentation
### Ομάδα: -
![alt text](logo-1.png)
Για την υλοποίηση του application επιλέξαμε το .NET Framework με το port της Lucene για C#, το Lucene.NET. Επίσης χρησιμοποιήσαμε ένα βοηθητικό πακέτο για HTML formating. Τα nuget πακέτα που χρησιμοποιήσαμε: Lucene.Net, Lucene.Net.Analysis.Common, Lucene.Net.Highlighter, Lucene.Net.Memory, Lucene.Net.Queries, Lucene.Net.QueryParser, Lucene.Net.Sandbox, HtmlRenderer.WinForms

Η δομή του project είναι ως εξής:
```
/information-retrieval/
    |
    Graphs.xlsx
    |
    lucengine/
        |
        Benchmark.cs
        |
        CACMAnalyzer.cs
        |
        Form1.cs
        |
        Indexer.cs
        |
        Searcher.cs
        |
        Globals.cs
        |
        ...
        |
        cacm/
            |
            [αρχεία εκφώνησης]
```

Οι μέθοδοι που διαχειρίζονται την παραγωγή των documents και τη δημιουργία του Index βρίσκονται στην κλάση Indexer, οι μέθοδοι που κάνουν αναζήτηση πάνω στο Index ανήκουν στην κλάση Searcher, οι μέθοδοι που εξάγουν τα στατιστικά για την κατασκευή των γραφημάτων βρίσκονται στην κλάση Benchmark και οι μέθοδοι που χειρίζονται το γραφικό περιβάλλον βρίσκονται στην κλάση Form1 (που είναι το κύριο περιβάλλον του προγράμματος).

Η κλάση Globals χρησιμοποιείται για να αποθηκεύονται τα directories, ο τρέχων Analyzer και το μοντέλο που χρησιμοποιείται. Αυτά αποθηκεύονται στις εξής μεταβλητές:
```cs
bool useBM25
RAMDirectory StandardDir
RAMDirectory BM25Dir
CACMAnalyzer Analyzer
```  

### CACMAnalyzer.cs
Ο StandardAnalyzer δεν έχει τη δυνατότητα να τον ρυθμίσουμε ώστε να χρησιμοποιείται ο αλγόριθμος Porter για stemming, οπότε έπρεπε να δημιουργήσουμε έναν νέο analyzer. O CACMAnalyzer πρέπει επίσης να χρησιμοποιεί και τα filters που χρησιμοποιεί ο StandardAnalyzer.

```cs
public CACMAnalyzer(CharArraySet stopWords)
```
O constructor του CACMAnalyzer δέχεται σαν παράμετρο ένα CharArraySet stopWords, που παράγεται από το αρχείο common_words.txt.

```cs
protected override TokenStreamCompenents CreateComponents(string fieldName, TextReader reader)
```
Για κάθε νέο field που περνά από τον Analyzer μέσω του reader, ο CACMAnalyzer πρώτα το κάνει tokenize με τον StandardTokenizer και έπειτα εφαρμόζει το LowerCaseFilter. Έπειτα παίρνοντας τα stopWords που έθεσε ο constructor, εφαρμόζει το StopFilter και τέλος, το PorterStemFilter στο stream έχει παραχθεί.

### Indexer.cs
Οι μέθοδοι της Indexer αφορούν όλες τη δημιουργία του Index. Η ParceCACM και η Create Document είναι και οι δύο βοηθητικές για την CreateIndex.
```cs
public static void CreateIndex()
```
Η CreateIndex αρχικά διαβάζει τη global μεταβλητή Globals.useBM25 (ώστε να διαγράψει τυχόν υπάρχοντα directories) και έπειτα δημιουργεί ενα νέο RAMDirectory. Το αρχείο common_words.txt διαβάζεται με έναν StreamReader ώστε να δημιουργηθεί το CharArraySet για τον CACMAnalyzer με τη βοήθεια της WordlistLoader.GetWordSet(). Μετά από αυτό αρχικοποιείται ο CACMAnalyzer και αποθηκεύεται στη global μεταβλητή του. Αφού δημιουργηθεί το IndexWriterConfig, επιλέγεται μοντέλο με βάση την Globals.useBM25 και με αυτό το config αρχικοποποιείται ο IndexWriter. Για να προστεθούν τα Documents στο Index, καλείται επαναληπτικά η ParseCACM() με το την τοποθεσία του αρχείου cacm.all, και μετά οι αλλαγές αποθηκεύονται με την IndexWriter.Commit(). Το directory αποηθηκεύεται στην αντίστοιχη global μεταβλητή.
```cs
private static IEnumerable<Document> ParseCACM(string path)
```
Η ParseCACM δέχεται την τοποθεσία του αρχείου cacm.all και επιστρέφει επαναληπτικά αντικείμενα τύπου Document. Για να αποθηκεύται η πληροφορία των fields (title, authors, abstract) χρησιμοποιούνται StringBuilders και για να ελέγχεται σε ποιο buffer προστίθεται η πληροφορία της επόμενης γραμμής, χρησιμοποιείται η μεταβλητή string nowReading. Μόλις εντοπιστεί η συμβολοσειρά ".I", το προηγούμενο Document περνάει στην έξοδο και τα buffers αδειάζουν για να σχηματιστεί το επόμενο, με το id που διαβάστηκε στην ίδια γραμμή. Για τις συμβολοσειρές ".T",".A",".W" η nowReading αλλάζει αντίστοιχα ενώ για άλλες που ξεκινούν με ".", δεν υπάρχει ανταπόκριση. Για την παραγωγή των Document χρησιμοποιείται η CreateDocument().
```cs
private static Document CreateDocument(string ID, string title, string authors, string abstractText)
```
H CreateDocument παίρνει ως είσοδο σε strings τα id, title, authors και abstractText που διαβάστηκαν από την ParseCACM και δημιουργεί ένα κενό αντικείμενο Document. Έπειτα ρυθμίζεται το FieldType (σύμφωνα με τις διαφάνειες) που χρησιμοποιείται αργότερα για τα Fields title, authors και abstract. Το id γίνεται StringField με Field.Store.YES και προσθέτουμε ένα ακόμη field "content" ως TextField με Field.Store.NO που περιέχει όλα τα string μαζί, για την αναζήτηση. Στο τέλος τα fields προστίθενται στο Document και επιστρέφεται.

### Searcher.cs
Η Searcher περιέχει δύο public methods, τη SearchIndex και τη MoreLikeThis. Η SearchIndex χρησιμοποιείται στη μπάρα αναζήτησης και από τη Benchmark.cs, ενώ η MoreLikeThis χρησιμοποιείται στα κουμπιά "More like this" δίπλα από τα άρθρα (όμοια με αυτό).
```cs
public static List<Document> SearchIndex(string q, int topNo)
```
Η SearchIndex δέχεται ως παραμέτρους το query της αναζήτησης σε string και τον αριθμό των αποτελεσμάτων (topNo) σε integer, και επιστρέφει τα Documents που ανακτήθηκαν σε List. Αρχικά φορτώνεται ανάλογα με το μοντέλο που χρησιμοποιείται το αντίστοιχο directory και δημιουργείται το List στο οποίο θα προστεθούν τα Documents. Δημιουργείται ένας DirectoryReader που ανοίγει το directory, και από αυτόν αρχικοποιείται ο IndexSearcher. Το Similarity ρυθμίζεται ανάλογα με το ποιο μοντέλο χρησιμοποείται. Έπειτα αρχικοποιείται ένας QueryParser για να επεξεργαστεί το string του query, παίρνοντας ως παραμέτρους το field στο οποίο θα γίνει αναζήτηση ("content") και τον Analyzer που δημιουργήθηκε στο indexing. Εάν το query έχει ειδικούς χαρακτήρες, χρησιμοποείται η QueryParser με παράμετρο QueryParser.Escape(q). Για την ανάκτηση των Scoredocs[] hits, χρησιμοποιείται η IndexSearcher.Search() με παραμέτρους το Query query (μετά το parsing), null filters και τον αριθμό topNO για τα αποτελέσματα. Επαναληπτικά για κάθε αντικείμενο ScoreDoc: εξάγεται το αντίστοιχο Document με την IndexSearcher.Doc(), προστίθεται ένα επιπλέον Field "LuceneID" (θα χρησιμοποιηθεί από την MoreLikeThis()) το οποίο περιέχει το εσωτερικό ID που απονέμει η Lucene σε ένα Document με Field.Store.YES και το Document προστίθεται στο List των αποτελεσμάτων.
```cs
public static List<Document> MoreLikeThis(int ID)
```
H MoreLikeThis() δέχεται ως παράμετρο το internal ID ενός Document σε μορφή integer. Αρχικά φορτώνεται το αντίστοιχο directory (ανάλογα με το μοντέλο που χρησιμοποιείται). Έπειτα όπως και στην SearchIndex, δημιουργείται ένα List για τα αποτελέσματα και αρχικοποιείται ο DirectoryReader και από αυτόν ο IndexSearcher (πάλι ανάλογα με το μοντέλο που χρησιμοποιείται). Για να ανακτηθούν συναφή Documents, δημιουργείται ένα αντικείμενο της κλάσης MoreLikeThis που χρησιμοποιεί τον Analyzer από το indexing, και χρησιμοποιεί τα "title", "authors" και "abstract" FieldNames. Το Query query που θα επιστρέψει τα όμοια Documents χτίζεται από τη συνάρτηση MoreLikeThis.Like() και εισάγεται στην IndexSearcher.Search(). Αποφασίσαμε κάθε φορά να επιστρέφουμε 10 παρόμοια Documents. Στα hits που επιστρέφονται, ακολουθείται επαναληπτικά η ίδια διαδικασία με την SearchIndex, αλλά εάν το αρχικό Document εμφανιστεί στα αποτελέσματα, παραβλέπεται.

### Benchmark.cs
Η κλάση Benchmark περιέχει τις μεθόδους που μετρούν την απόδοση στην ανάκτηση για κάθε μοντέλο και εξάγουν τα αποτελέσματα σε αρχεία .csv ώστε να σχηματιστούν τα γραφήματα. Ορίζουμε την κλάση BenchmarkQuery{int ID, string text} για να αποθηκεύουμε τα queries από το αρχείο query.text.

```cs
public static List<BenchmarkQuery> LoadQueries()
```

#### Σημειώσεις:
- Τα αρχεία query.text και qrels.text μετονομάστηκαν σε query.txt και qrels_alt.txt. Το qrels_alt.txt ανοίχτηκε πρώτα στο Excel για να διορθωθούν θέματα στοίχισης.
- Το αρχείο qrels.text δεν είχε συναφή Documents για αρκετά από τα queries οπότε δεν σχηματίστηκαν τα γραφήματά τους.

