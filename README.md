# Ανάκτηση Πληροφορίας και Αναζήτηση στον Παγκόσμιο Ιστό - Απαλλακτική Εργασία - Documentation
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
Η κλάση Benchmark περιέχει τις μεθόδους που μετρούν την απόδοση στην ανάκτηση για κάθε μοντέλο και εξάγουν τα αποτελέσματα σε αρχεία .csv ώστε να σχηματιστούν τα γραφήματα. Ορίζουμε την κλάση BenchmarkQuery{int ID, string text} για να αποθηκεύουμε τα queries από το αρχείο query.text. Οι μέθοδοι LoadQueries και LoadRelevant είναι βοηθητικές για την Evaluate.

```cs
public static List<BenchmarkQuery> LoadQueries()
```
Η LoadQueries επιστρέφει ένα List τύπου BenchmarkQuery με όλα τα queries που διάβασε από το αρχείο query.text. Αρχικά ορίζεται το List στο οποίο θα αποθηκευτούν και αρχικοποιούνται μια boolean μεταβλητή ελέγχου για να γνωρίζει ο reader εάν υπάρχει query σε εκείνη τη γραμμή, ένα StringBuilder ως buffer για το κείμενό της και ένα αντικείμενο BenchmarkQuery. Επαναληπτικά, για κάθε γραμμή του αρχείου, μόλις εντοπιστεί η συμβολοσειρά ".", ελέγχεται αν είναι ".Ι", οπότε το προηγούμενο BenchmarkQuery αποθηκεύεται στο List και δημιουργείται νέο, ή ".W", οπότε η μεταβλητή ελέγχου γίνεται αληθής και η επόμενη γραμμή θα συνενωθεί στο StringBuilder του query. Οι υπόλοιπες συμβολοσειρές που ξεκινούν με "." δε μας ενδιαφέρουν.
```cs
public static Dictionary<int, HashSet<int>> LoadRelevant()
```
H LoadRelevant επιστρέφει ένα Dictionary με key τον integer που αντιπροσωπεύει το ID ενός query και value ένα HashSet τύπου integer για τα ID των συναφή Documents με το αντίστοιχο query. Αφού ανοιχτεί το qrels.text, επαναληπτικά κάθε γραμμή χωρίζεται σε έναν πίνακα συμβολοσειρών (["12","5354",...]) όπου το πρώτο στοιχείο αποθηκεύεται ως int στη θέση key στο dictionary και το δεύτερο ως int στη αντίστοιχο HashSet της θέσης value.
```cs
public static void Evaluate()
```
H Evaluate γράφει τις τιμές που χρειάζονται για τα γραφήματα ακρίβειας-ανάκλησης κάθε query με το μοντέλο που χρησιμοποιείται εκείνη τη στιγμή απο το application σε ένα αρχείο .csv. Αρχικά, ένα List και ένα Dictionary χρησιμοποιούνται για να φορτωθούν τα queries και τα συναφή Documents και αρχικοποιείται ένα StringBuilder για το περιεχόμενο του .csv. Επαναληπτικά για κάθε query, υπολογίζεται ο αριθμός των συναφών Documents από το qrels.text και μέσω της Searcher.SearchIndex() ανακτάται ένα List με τα Documents που επέστρεψε η αναζήτηση με βάση το query μέσα στο application. Επιλέξαμε να ζητήσουμε τα πρώτα 50 αποτελέσματα. Για κάθε αποτέλεσμα ενός query, δημιουργείται προσωρινά ένα Tuple<double,double> δύο πραγματικών αριθμών και εάν το ID του τρέχοντος Document περιέχεται στα συναφή, ο αριθμός των συναφών που ανακτήθηκαν αυξάνεται. Υπολογίζονται τα 
$precision=\frac{relevant so far}{total retrieved}$ και $recall=\frac{relevant so far}{total relevant from file}$ και προστίθενται σε ένα List. Έπειτα, σε ένα βρόχο 11 επαναλήψεων, εφαρμόζουμε τον τύπο $Pinterp(r)=max(r'\geq r)p(r')$ για να σχηματιστούν τα 11 σημεία της καμπύλης. Μετά προστίθεται μια γραμμή {queryID,r,Pinterp} στο αρχείο .csv

### Form1.cs
Σε αυτή την κλάση ορίζονται όλες οι μέθοδοι που συνδέουν τις προηγούμενες κλάσεις με το γραφικό περιβάλλον.
```cs
private void DisplayResults(string query, int topNo)
```
Η DisplayResults καλείται όταν πατιέται το Search button, και δέχεται ως παραμέτρους το string του query από το searchbar και έναν integer για τον αριθμό αποτελεσμάτων. Αρχικά το FlowLayoutPanel των αποτελεσμάτων καθαρίζει από προηγούμενα άρθρα και αφού κληθεί η Searcher.SearchIndex() με τις παραμέτρους που δόθηκαν, για κάθε Document καλείται η QueryParser.Parse() και μετά η DrawArticlePanel() με παραμέτρους το Document και το query (ώστε να γίνει η υπογράμμιση). Έπειτα το Panel προστίθεται στο FlowLayoutPanel των αποτελεσμάτων.
```cs
private Panel DrawArticlePanel(Document doc, Query query)
```
Η DrawArticlePanel δέχεται ως παραμέτρους ένα Document και το query από το οποίο ανακτήθηκε και επιστρέφει το Panel του. Αφότου εξάγει τις βασικές πληροφορίες από το Document σε strings, καλεί για κάθε ένα από τα Fields την HighlightKeywords() περνώντας τα ως παραμέτρους μαζί με το query. Τα αποτελέσματα αποθηκεύονται σε HtmlLabels. Προστίθεται επίσης ένα button "More like this" στο οποίο δεσμεύεται η DisplaySimilarResults() και το Tag του περιέχει το εσωτερικό ID του Document.
```cs
private string HighlightKeywords(string rawText, Query query, bool isAbstract)
```
Η HighlightKeywords δέχεται ως παραμέτρους κείμενο σε string, ένα αντικείμενο Query και μία boolean μεταβλητή που υποδεικνύει αν το κείμενο είναι abstract. Δημιουργούνται ένας SimpleHTMLFormatter με (με dummy συμβολοσειρές που περικλύουν ένα keyword), ένας QueryScorer από το query, ένας Highlighter (από τον formatter και τον scorer) αρχικοποιείται ο TextFragmenter του. Με τη βοήθεια του Analyzer που έχει οριστεί στο indexing, δημιουργείται ένα TokenStream από το κείμενο. Από αυτό παράγεται το επισημασμένο κείμενο μέσω της Highlighter.GetBestFragment(). Μετά από αυτό, εάν δεν υπήρχαν επισημάνσεις στο κείμενο και είναι αρκετά μεγάλο, κόβεται στους 200 χαρακτήρες. Εάν είναι abstract, κρατάμε την πρώτη εμφάνιση ενός keyword και το υπόλοιπο κόβεται επίσης. Στο τέλος οι dummy συμβολοσειρές γύρω από τα keywords αντικαθίστανται με css.
```cs
private void DisplaySimilarResults(object sender, EventArgs e)
```
Η DisplaySimilarResults καλείται κάθε φορά που πατιέται ένα button "More like this" δίπλα σε κάποιο άρθρο. Αφού αφαιρεθούν όλα τα Panel που υπάρχουν εκείνη τη στιγμή στο FlowLayoutPanel, καλείται η Searcher.MoreLikeThis() με παράμετρο το ID που ανακτάται από το button. Εάν το List με τα Documents που επιστράφηκε δεν είναι κενό, προστίθενται επαναληπτικά στο FlowLayoutPanel μέσω της DrawArticlePanel().

#### Σημειώσεις:
- Τα αρχεία query.text και qrels.text μετονομάστηκαν σε query.txt και qrels_alt.txt. Το qrels_alt.txt ανοίχτηκε πρώτα στο Excel για να διορθωθούν θέματα στοίχισης.
- Το αρχείο qrels.text δεν είχε συναφή Documents για αρκετά από τα queries οπότε δεν σχηματίστηκαν τα γραφήματά τους.

